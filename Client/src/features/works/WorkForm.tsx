import { zodResolver } from "@hookform/resolvers/zod";
import {
  Autocomplete,
  Button,
  Chip,
  CircularProgress,
  Grid,
  MenuItem,
  TextField,
} from "@mui/material";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { useQuery } from "@tanstack/react-query";
import { vi } from "date-fns/locale";
import { useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";
import { useAuth } from "../../app/shared/contexts/AuthContext";
import { useWorkFormData } from "../../hooks/useWorkData";
import { getAuthorRolesByWorkTypeId } from "../../lib/api/authorRolesApi";
import { getPurposesByWorkTypeId } from "../../lib/api/purposesApi";
import { getScimagoFields } from "../../lib/api/scimagoFieldsApi";
import { getScoreLevelsByFilters } from "../../lib/api/scoreLevelsApi";
import { getUserById, searchUsers } from "../../lib/api/usersApi";
import { getWorkLevelsByWorkTypeId } from "../../lib/api/workLevelsApi";
import { WorkSource } from "../../lib/types/enums/WorkSource";
import { User } from "../../lib/types/models/User";
import { Work } from "../../lib/types/models/Work";
import { getScoreLevelFullDescription } from "../../lib/utils/scoreLevelUtils";
import { FieldDef, workDetailsConfig } from "../../lib/utils/workDetailsConfig";

// Define validation schema
const schema = z.object({
  title: z.string().min(2, "Tên công trình phải có ít nhất 2 ký tự"),
  timePublished: z.any().optional().nullable(),
  totalAuthors: z.coerce
    .number()
    .min(1, "Số tác giả phải lớn hơn 0")
    .optional()
    .nullable(),
  totalMainAuthors: z.coerce
    .number()
    .min(1, "Số tác giả chính phải lớn hơn 0")
    .optional()
    .nullable(),
  details: z.any().optional(),
  source: z.number(),
  workTypeId: z.string().uuid("ID loại công trình không hợp lệ"),
  workLevelId: z
    .string()
    .uuid("ID cấp độ công trình không hợp lệ")
    .optional()
    .nullable(),
  coAuthorUserIds: z
    .array(z.string().uuid("ID đồng tác giả không hợp lệ"))
    .optional()
    .default([]),
  author: z.object({
    authorRoleId: z
      .string()
      .uuid("ID vai trò tác giả không hợp lệ")
      .optional()
      .nullable(),
    purposeId: z.string().uuid("ID mục đích không hợp lệ"),
    position: z
      .number()
      .min(1, "Vị trí tác giả phải lớn hơn 0")
      .optional()
      .nullable(),
    scoreLevel: z
      .number()
      .min(0, "Mức điểm không hợp lệ")
      .optional()
      .nullable(),
    sCImagoFieldId: z
      .string()
      .uuid("ID lĩnh vực SCImago không hợp lệ")
      .optional()
      .nullable(),
    fieldId: z.string().uuid("ID lĩnh vực không hợp lệ").optional().nullable(),
  }),
});

type WorkFormData = z.infer<typeof schema>;

interface WorkFormProps {
  initialData?: Work | null;
  onSubmit: (data: WorkFormData) => void;
  isLoading?: boolean;
  workTypes: Array<{ id: string; name: string }>;
  workLevels: Array<{ id: string; name: string }>;
  authorRoles: Array<{ id: string; name: string }>;
  purposes: Array<{ id: string; name: string }>;
  scimagoFields: Array<{ id: string; name: string }>;
  fields: Array<{ id: string; name: string }>;
  activeTab: number;
  setActiveTab: (index: number) => void;
}

export default function WorkForm({
  initialData,
  onSubmit,
  isLoading,
  workTypes,
  fields,
  activeTab,
  setActiveTab,
}: WorkFormProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCoAuthors, setSelectedCoAuthors] = useState<User[]>([]);
  const [detailsFields, setDetailsFields] = useState<Record<string, string>>(
    {},
  );
  const [isLoadingCoAuthors, setIsLoadingCoAuthors] = useState(false);
  const [visibleScoreLevels, setVisibleScoreLevels] = useState<number[]>([]);

  const { user } = useAuth();

  // Lấy userId từ localStorage hoặc context auth nếu có
  // Trong ứng dụng thực tế, bạn nên sử dụng hook auth chính thức
  const currentUserId = user?.id;

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
    watch,
  } = useForm<WorkFormData>({
    resolver: zodResolver(schema),
    mode: "onSubmit",
    defaultValues: initialData
      ? {
          ...initialData,
          timePublished: initialData.timePublished
            ? new Date(initialData.timePublished)
            : null,
          details: initialData.details || {},
          source: initialData.source,
          coAuthorUserIds:
            initialData.coAuthorUserIds?.filter(
              (id) => id.toString() !== currentUserId,
            ) || [],
          author: {
            authorRoleId: initialData.author?.authorRoleId || null,
            purposeId: initialData.author?.purposeId || "",
            position: initialData.author?.position || 1,
            scoreLevel: initialData.author?.scoreLevel ?? null,
            sCImagoFieldId: initialData.author?.scImagoFieldId || null,
            fieldId: initialData.author?.fieldId || null,
          },
        }
      : {
          title: "",
          timePublished: null,
          totalAuthors: null,
          totalMainAuthors: null,
          details: {},
          source: WorkSource.NguoiDungKeKhai,
          workTypeId: "",
          workLevelId: null,
          coAuthorUserIds: [],
          author: {
            authorRoleId: null,
            purposeId: "",
            position: 1,
            scoreLevel: null,
            sCImagoFieldId: null,
            fieldId: null,
          },
        },
  });

  const { workTypes: formWorkTypes, workLevels: formWorkLevels } =
    useWorkFormData();
  // Watch workTypeId để lấy dữ liệu phù hợp
  const workTypeId = watch("workTypeId");
  const workLevelId = watch("workLevelId");
  const authorRoleId = watch("author.authorRoleId");
  const purposeId = watch("author.purposeId");

  // Khởi tạo các trường details dựa trên initialData
  useEffect(() => {
    if (initialData?.details) {
      setDetailsFields(initialData.details);
    } else {
      setDetailsFields({});
    }
  }, [initialData]);

  // Xác định các trường details cần hiển thị dựa trên loại công trình và cấp công trình
  const workTypeName = useMemo(
    () => formWorkTypes.find((t) => t.id === workTypeId)?.name,
    [workTypeId, formWorkTypes],
  );
  const workLevelName = useMemo(
    () => formWorkLevels.find((l) => l.id === workLevelId)?.name,
    [workLevelId, formWorkLevels],
  );

  const detailsConfig: FieldDef[] = useMemo(() => {
    if (!workTypeName) return [];
    const cfg = workDetailsConfig[workTypeName.toLowerCase()];
    if (!cfg) return [];
    // if there's a specific level override, use it, otherwise default
    return cfg[workLevelName!] || cfg.default;
  }, [workTypeName, workLevelName]);

  // Cập nhật details khi người dùng nhập vào các trường
  const handleDetailsFieldChange = (key: string, value: string) => {
    const updatedFields = { ...detailsFields, [key]: value };
    setDetailsFields(updatedFields);
    setValue("details", updatedFields);
  };

  // Fetch dữ liệu dựa trên workTypeId
  const { data: workLevelsData } = useQuery({
    queryKey: ["workLevels", workTypeId],
    queryFn: () => getWorkLevelsByWorkTypeId(workTypeId),
    enabled: !!workTypeId,
  });

  const { data: authorRolesData } = useQuery({
    queryKey: ["authorRoles", workTypeId],
    queryFn: () => getAuthorRolesByWorkTypeId(workTypeId),
    enabled: !!workTypeId,
  });

  const { data: purposesData } = useQuery({
    queryKey: ["purposes", workTypeId],
    queryFn: () => getPurposesByWorkTypeId(workTypeId),
    enabled: !!workTypeId,
  });

  const { data: scimagoFieldsData, isLoading: isLoadingScimagoFields } = useQuery({
    queryKey: ["scimagoFields"],
    queryFn: getScimagoFields,
    enabled: !!workTypeId
  });

  // Lọc scimagoFields dựa trên workTypeId
  const filteredScimagoFields = useMemo(() => {
    if (!scimagoFieldsData?.data) return [];
    return scimagoFieldsData.data;
  }, [scimagoFieldsData?.data]);

  // Tìm kiếm người dùng
  const { data: usersData, isLoading: isLoadingUsers } = useQuery({
    queryKey: ["searchUsers", searchTerm],
    queryFn: async () => {
      const response = await searchUsers(searchTerm);
      console.log("Kết quả tìm kiếm người dùng:", response);
      return response;
    },
    enabled: searchTerm.length > 0,
  });

  // Lọc ra người dùng hiện tại khỏi kết quả tìm kiếm
  const filteredUsers = useMemo(() => {
    if (!usersData?.data) return [];
    return usersData.data.filter((user) => user.id !== currentUserId);
  }, [usersData?.data, currentUserId]);

  // Load dữ liệu từ initialData khi nó thay đổi
  useEffect(() => {
    if (initialData) {
      console.log("initialData đã thay đổi:", initialData);
      console.log("coAuthorUserIds:", initialData.coAuthorUserIds);

      // Reset form với dữ liệu từ initialData
      reset({
        ...initialData,
        timePublished: initialData.timePublished
          ? new Date(initialData.timePublished)
          : null,
        details: initialData.details || {},
        source: initialData.source,
        coAuthorUserIds:
          initialData.coAuthorUserIds?.filter(
            (id) => id.toString() !== currentUserId,
          ) || [],
        author: {
          authorRoleId: initialData.author?.authorRoleId || null,
          purposeId: initialData.author?.purposeId || "",
          position: initialData.author?.position || 1,
          scoreLevel: initialData.author?.scoreLevel ?? null,
          sCImagoFieldId: initialData.author?.scImagoFieldId || null,
          fieldId: initialData.author?.fieldId || null,
        },
      });

      // Tải dữ liệu đồng tác giả
      const loadCoAuthors = async () => {
        if (
          initialData.coAuthorUserIds &&
          initialData.coAuthorUserIds.length > 0
        ) {
          setIsLoadingCoAuthors(true);
          try {
            const coAuthorsData: User[] = [];
            for (const userId of initialData.coAuthorUserIds) {
              if (userId.toString() !== currentUserId) {
                const response = await getUserById(userId);
                if (response.success && response.data) {
                  coAuthorsData.push(response.data);
                }
              }
            }
            setSelectedCoAuthors(coAuthorsData);
          } catch (error) {
            console.error("Lỗi khi tải dữ liệu đồng tác giả:", error);
          } finally {
            setIsLoadingCoAuthors(false);
          }
        } else {
          setSelectedCoAuthors([]);
        }
      };

      loadCoAuthors();
    }
  }, [initialData, reset, currentUserId]);

  // Kiểm tra và thiết lập các mức điểm (scoreLevel) hiển thị dựa trên loại công trình và cấp công trình
  useEffect(() => {
    if (!workTypeId) {
      setVisibleScoreLevels([]);
      return;
    }

    const fetchScoreLevels = async () => {
      try {
        // Gọi API để lấy danh sách mức điểm từ backend
        const scoreLevels = await getScoreLevelsByFilters(
          workTypeId,
          workLevelId || undefined,
          authorRoleId || undefined,
          purposeId || undefined,
        );

        console.log("Đã lấy được danh sách mức điểm:", scoreLevels);
        setVisibleScoreLevels(scoreLevels);

        // Nếu không có mức điểm nào, reset giá trị scoreLevel về null
        if (scoreLevels.length === 0) {
          setValue("author.scoreLevel", null);
        }
      } catch (error) {
        console.error("Lỗi khi lấy danh sách mức điểm:", error);
        setVisibleScoreLevels([]);
        setValue("author.scoreLevel", null);
      }
    };

    fetchScoreLevels();
  }, [workTypeId, workLevelId, authorRoleId, purposeId, setValue]);

  // Kiểm tra xem loại công trình có cấp công trình hay không
  const [hasWorkLevels, setHasWorkLevels] = useState(true);

  // Kiểm tra loại công trình có cấp độ hay không
  useEffect(() => {
    if (workTypeId) {
      // Kiểm tra xem có dữ liệu workLevels không
      if (workLevelsData?.data && Array.isArray(workLevelsData.data)) {
        setHasWorkLevels(workLevelsData.data.length > 0);

        // Nếu không có cấp công trình, đặt workLevelId thành null
        if (workLevelsData.data.length === 0) {
          setValue("workLevelId", null);
        }
      } else {
        // Mặc định là có cấp công trình
        setHasWorkLevels(true);
      }
    }
  }, [workTypeId, workLevelsData, setValue]);

  // Reset các trường phụ thuộc khi workTypeId thay đổi và không có initialData
  useEffect(() => {
    if (workTypeId && !initialData) {
      setValue("workLevelId", null);
      setValue("author.authorRoleId", null);
      setValue("author.purposeId", "");
      setValue("author.sCImagoFieldId", null);
    }
  }, [workTypeId, setValue, initialData]);

  // Cập nhật coAuthorUserIds khi selectedCoAuthors thay đổi
  useEffect(() => {
    if (!selectedCoAuthors) return;

    console.log("Cập nhật coAuthorUserIds với:", selectedCoAuthors);
    const coAuthorIds = selectedCoAuthors.map((user) => user.id);
    console.log("Danh sách ID đồng tác giả cập nhật:", coAuthorIds);

    // Đảm bảo cập nhật giá trị vào form
    setValue("coAuthorUserIds", coAuthorIds, { shouldValidate: true });
  }, [selectedCoAuthors, setValue]);

  // Xử lý chuyển đổi giá trị dạng chuỗi sang số
  useEffect(() => {
    const totalAuthors = watch("totalAuthors");
    const totalMainAuthors = watch("totalMainAuthors");

    // Kiểm tra nếu giá trị là chuỗi, chuyển thành số
    if (totalAuthors !== null && typeof totalAuthors === "string") {
      setValue("totalAuthors", Number(totalAuthors));
    }

    if (totalMainAuthors !== null && typeof totalMainAuthors === "string") {
      setValue("totalMainAuthors", Number(totalMainAuthors));
    }
  }, [watch, setValue]);

  // Xử lý chuyển đổi details object sang string và ngược lại
  useEffect(() => {
    if (initialData?.details) {
      try {
        // Khởi tạo trường details từ dữ liệu ban đầu
        setDetailsFields(initialData.details);
      } catch (error) {
        console.error("Lỗi khi parse details:", error);
        setDetailsFields({});
      }
    } else {
      setDetailsFields({});
    }
  }, [initialData]);

  // Xử lý khi người dùng nhập chi tiết
  // const handleDetailsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
  //   try {
  //     const jsonValue = JSON.parse(e.target.value);
  //     setDetailsFields(jsonValue);
  //     setValue("details", jsonValue);
  //   } catch (error) {
  //     console.error("Lỗi khi parse JSON:", error);
  //   }
  // };

  // Xử lý khi đồng tác giả thay đổi
  const handleCoAuthorChange = (_: any, newValue: User[]) => {
    console.log("Đã chọn đồng tác giả:", newValue);

    // Đảm bảo không có người dùng hiện tại trong danh sách đồng tác giả
    const filteredCoAuthors = newValue.filter(
      (user) => user.id !== currentUserId,
    );

    setSelectedCoAuthors(filteredCoAuthors);

    // Cập nhật giá trị trong form - chỉ lưu trữ IDs
    const coAuthorIds = filteredCoAuthors.map((user) => user.id);
    console.log("Danh sách ID đồng tác giả cập nhật:", coAuthorIds);

    setValue("coAuthorUserIds", coAuthorIds, { shouldValidate: true });
  };

  const handleFormErrors = (formErrors) => {
    console.warn("Validation failed:", formErrors);

    const tab1Fields = [
      "title",
      "timePublished",
      "totalAuthors",
      "totalMainAuthors",
      "workTypeId",
      "workLevelId",
      "coAuthorUserIds",
      "details",
    ];
    const tab2Fields = ["author"];

    const hasTab1Error = Object.keys(formErrors).some((field) =>
      tab1Fields.includes(field),
    );
    const hasTab2Error = Object.keys(formErrors).some((field) =>
      tab2Fields.includes(field),
    );

    if (hasTab1Error && activeTab !== 0) {
      alert("Có lỗi ở tab Thông tin công trình. Vui lòng kiểm tra lại.");
      setActiveTab(0);
    } else if (hasTab2Error && activeTab !== 1) {
      alert("Có lỗi ở tab Tác giả. Vui lòng kiểm tra lại.");
      setActiveTab(1);
    }
  };

  return (
    <form onSubmit={handleSubmit(onSubmit, handleFormErrors)}>
      <Grid container spacing={2}>
        {activeTab === 0 && (
          // Tab thông tin công trình
          <>
            <Grid item xs={6}>
              <Controller
                name="title"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    label="Tên công trình"
                    fullWidth
                    error={!!errors.title}
                    helperText={errors.title?.message?.toString()}
                    disabled={initialData?.isLocked}
                  />
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="timePublished"
                control={control}
                render={({ field }) => (
                  <LocalizationProvider
                    dateAdapter={AdapterDateFns}
                    adapterLocale={vi}
                  >
                    <DatePicker
                      label="Thời gian xuất bản"
                      value={field.value ? new Date(field.value) : null}
                      onChange={(date) => field.onChange(date)}
                      disabled={initialData?.isLocked}
                      slotProps={{
                        textField: {
                          fullWidth: true,
                          error: !!errors.timePublished,
                          helperText: errors.timePublished?.message?.toString(),
                        },
                      }}
                    />
                  </LocalizationProvider>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="workTypeId"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    select
                    label="Loại công trình"
                    fullWidth
                    error={!!errors.workTypeId}
                    helperText={errors.workTypeId?.message?.toString()}
                    disabled={initialData?.isLocked}
                  >
                    {workTypes.map((type) => (
                      <MenuItem key={type.id} value={type.id}>
                        {type.name}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="workLevelId"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    select
                    label="Cấp công trình"
                    fullWidth
                    error={!!errors.workLevelId}
                    helperText={errors.workLevelId?.message?.toString()}
                    disabled={
                      !workTypeId || !hasWorkLevels || initialData?.isLocked
                    }
                  >
                    {workLevelsData?.data?.map((level) => (
                      <MenuItem key={level.id} value={level.id}>
                        {level.name}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="totalAuthors"
                control={control}
                render={({ field: { value, onChange, onBlur, ...field } }) => (
                  <TextField
                    {...field}
                    label="Tổng số tác giả"
                    type="number"
                    inputProps={{ min: 1, step: 1 }}
                    fullWidth
                    error={!!errors.totalAuthors}
                    helperText={errors.totalAuthors?.message?.toString()}
                    value={value === null ? "" : value}
                    onChange={(e) => {
                      const inputValue = e.target.value;
                      onChange(inputValue === "" ? null : Number(inputValue));
                    }}
                    onBlur={onBlur}
                    disabled={initialData?.isLocked}
                  />
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="totalMainAuthors"
                control={control}
                render={({ field: { value, onChange, onBlur, ...field } }) => (
                  <TextField
                    {...field}
                    label="Số tác giả chính"
                    type="number"
                    inputProps={{ min: 1, step: 1 }}
                    fullWidth
                    error={!!errors.totalMainAuthors}
                    helperText={errors.totalMainAuthors?.message?.toString()}
                    value={value === null ? "" : value}
                    onChange={(e) => {
                      const inputValue = e.target.value;
                      onChange(inputValue === "" ? null : Number(inputValue));
                    }}
                    onBlur={onBlur}
                    disabled={initialData?.isLocked}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12}>
              <Controller
                name="coAuthorUserIds"
                control={control}
                render={() => (
                  <Autocomplete
                    multiple
                    id="coAuthorUserIds"
                    options={filteredUsers}
                    getOptionLabel={(option) => {
                      if (typeof option === "string") return option;
                      return `${option.fullName} - ${option.userName}${option.departmentName ? ` - ${option.departmentName}` : ""}`;
                    }}
                    isOptionEqualToValue={(option, value) =>
                      option.id === value.id
                    }
                    value={selectedCoAuthors}
                    onChange={handleCoAuthorChange}
                    loading={isLoadingUsers || isLoadingCoAuthors}
                    disabled={initialData?.isLocked}
                    renderTags={(value, getTagProps) =>
                      value.map((option, index) => {
                        const props = getTagProps({ index });
                        return (
                          <Chip
                            {...props}
                            label={`${option.fullName} - ${option.userName}${option.departmentName ? ` - ${option.departmentName}` : ""}`}
                            size="medium"
                          />
                        );
                      })
                    }
                    renderInput={(params) => (
                      <TextField
                        {...params}
                        label="Đồng tác giả"
                        variant="outlined"
                        onChange={(e) => setSearchTerm(e.target.value)}
                        placeholder="Tìm kiếm đồng tác giả"
                        error={!!errors.coAuthorUserIds}
                        helperText={
                          errors.coAuthorUserIds?.message ||
                          "Thêm các đồng tác giả (không bao gồm bạn)"
                        }
                        InputProps={{
                          ...params.InputProps,
                          endAdornment: (
                            <>
                              {isLoadingUsers || isLoadingCoAuthors ? (
                                <CircularProgress color="inherit" size={20} />
                              ) : null}
                              {params.InputProps.endAdornment}
                            </>
                          ),
                        }}
                      />
                    )}
                  />
                )}
              />
            </Grid>

            {/* Chi tiết động dựa trên loại công trình và cấp công trình */}
            {detailsConfig.length > 0 && (
              <Grid item xs={12}>
                <Grid container spacing={2}>
                  {detailsConfig.map((field) => (
                    <Grid item xs={12} sm={6} key={field.key}>
                      <TextField
                        label={field.label}
                        fullWidth
                        value={detailsFields[field.key] || ""}
                        onChange={(e) =>
                          handleDetailsFieldChange(field.key, e.target.value)
                        }
                        required={field.required}
                        error={
                          field.required &&
                          (!detailsFields[field.key] ||
                            detailsFields[field.key].trim() === "")
                        }
                        helperText={
                          field.required &&
                          (!detailsFields[field.key] ||
                            detailsFields[field.key].trim() === "")
                            ? "Trường này là bắt buộc"
                            : ""
                        }
                        disabled={initialData?.isLocked}
                      />
                    </Grid>
                  ))}
                </Grid>
              </Grid>
            )}
          </>
        )}

        {activeTab === 1 && (
          // Tab thông tin tác giả
          <>
            <Grid item xs={6}>
              <Controller
                name="author.authorRoleId"
                control={control}
                render={({ field: { value, onChange, ...restField } }) => (
                  <TextField
                    {...restField}
                    select
                    label="Vai trò tác giả"
                    fullWidth
                    error={!!errors.author?.authorRoleId}
                    helperText={errors.author?.authorRoleId?.message?.toString()}
                    disabled={!workTypeId}
                    value={value ?? ""}
                    onChange={(e) => {
                      const newValue =
                        e.target.value === "" ? null : e.target.value;
                      onChange(newValue);
                    }}
                  >
                    <MenuItem value="">Chọn vai trò tác giả</MenuItem>
                    {authorRolesData?.data?.map((role) => (
                      <MenuItem key={role.id} value={role.id}>
                        {role.name}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="author.purposeId"
                control={control}
                render={({ field }) => (
                  <TextField
                    {...field}
                    select
                    label="Mục đích quy đổi"
                    fullWidth
                    error={!!errors.author?.purposeId}
                    helperText={errors.author?.purposeId?.message?.toString()}
                    disabled={!workTypeId}
                  >
                    {purposesData?.data?.map((purpose) => (
                      <MenuItem key={purpose.id} value={purpose.id}>
                        {purpose.name}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="author.position"
                control={control}
                render={({ field: { value, onChange, ...restField } }) => (
                  <TextField
                    {...restField}
                    label="Vị trí tác giả"
                    type="number"
                    fullWidth
                    error={!!errors.author?.position}
                    helperText={errors.author?.position?.message?.toString()}
                    value={value === null ? "" : value}
                    onChange={(e) => {
                      const inputValue = e.target.value;
                      onChange(inputValue === "" ? null : Number(inputValue));
                    }}
                  />
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="author.scoreLevel"
                control={control}
                render={({ field: { value, onChange, ...restField } }) => (
                  <TextField
                    {...restField}
                    select
                    label="Mức điểm"
                    fullWidth
                    error={!!errors.author?.scoreLevel}
                    helperText={errors.author?.scoreLevel?.message?.toString()}
                    disabled={visibleScoreLevels.length === 0}
                    value={value ?? ""}
                    onChange={(e) => {
                      const newValue =
                        e.target.value === "" ? null : Number(e.target.value);
                      onChange(newValue);
                    }}
                  >
                    <MenuItem value="">Chọn mức điểm</MenuItem>
                    {visibleScoreLevels.map((level) => (
                      <MenuItem key={level} value={level}>
                        {getScoreLevelFullDescription(level)}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="author.sCImagoFieldId"
                control={control}
                render={({ field: { value, onChange, ...restField } }) => (
                  <TextField
                    {...restField}
                    select
                    label="Ngành SCImago"
                    fullWidth
                    error={!!errors.author?.sCImagoFieldId}
                    helperText={errors.author?.sCImagoFieldId?.message?.toString()}
                    disabled={!workTypeId}
                    value={value ?? ""}
                    onChange={(e) => {
                      const newValue =
                        e.target.value === "" ? null : e.target.value;
                      onChange(newValue);
                    }}
                  >
                    <MenuItem value="">Chọn ngành SCImago</MenuItem>
                    {filteredScimagoFields.map((field) => (
                      <MenuItem key={field.id} value={field.id}>
                        {field.name}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="author.fieldId"
                control={control}
                render={({ field: { value, onChange, ...restField } }) => (
                  <TextField
                    {...restField}
                    select
                    label="Ngành tính điểm"
                    fullWidth
                    error={!!errors.author?.fieldId}
                    helperText={errors.author?.fieldId?.message?.toString()}
                    value={value ?? ""}
                    onChange={(e) => {
                      const newValue =
                        e.target.value === "" ? null : e.target.value;
                      onChange(newValue);
                    }}
                    disabled={false}
                  >
                    <MenuItem value="">Chọn ngành tính điểm</MenuItem>
                    {fields.map((field) => (
                      <MenuItem key={field.id} value={field.id}>
                        {field.name}
                      </MenuItem>
                    ))}
                  </TextField>
                )}
              />
            </Grid>
          </>
        )}

        <Grid item xs={12} sx={{ mt: 2 }}>
          <Button
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            disabled={isLoading}
          >
            {isLoading ? (
              <CircularProgress size={24} />
            ) : initialData ? (
              "Cập nhật"
            ) : (
              "Thêm mới"
            )}
          </Button>
        </Grid>
      </Grid>
    </form>
  );
}
