import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  DialogActions,
  TextField,
  Grid,
  MenuItem,
  CircularProgress,
  Box,
  Autocomplete,
  Chip,
  Stack,
  FormControlLabel,
  Checkbox
} from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { vi } from "date-fns/locale";
import { useEffect, useState, useMemo } from "react";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";
import { Work } from "../../../lib/types/models/Work";
import { WorkSource } from "../../../lib/types/enums/WorkSource";
import { ScoreLevel } from "../../../lib/types/enums/ScoreLevel";
import { getWorkLevelsByWorkTypeId } from "../../../lib/api/workLevelsApi";
import { getAuthorRolesByWorkTypeId } from "../../../lib/api/authorRolesApi";
import { getPurposesByWorkTypeId } from "../../../lib/api/purposesApi";
import { getScimagoFieldsByWorkTypeId } from "../../../lib/api/scimagoFieldsApi";
import { searchUsers, getUserById } from "../../../lib/api/usersApi";
import { User } from "../../../lib/types/models/User";
import { useQuery } from "@tanstack/react-query";

// Define validation schema
const schema = z.object({
  title: z.string().min(2, "Tên công trình phải có ít nhất 2 ký tự"),
  timePublished: z.any().optional().nullable(),
  totalAuthors: z.coerce.number().min(1, "Số tác giả phải lớn hơn 0").optional().nullable(),
  totalMainAuthors: z.coerce.number().min(1, "Số tác giả chính phải lớn hơn 0").optional().nullable(),
  details: z.any().optional(),
  source: z.number(),
  workTypeId: z.string().uuid("ID loại công trình không hợp lệ"),
  workLevelId: z.string().uuid("ID cấp độ công trình không hợp lệ").optional().nullable(),
  author: z.object({
    authorRoleId: z.string().uuid("ID vai trò tác giả không hợp lệ"),
    purposeId: z.string().uuid("ID mục đích không hợp lệ"),
    position: z.number().min(1, "Vị trí tác giả phải lớn hơn 0").optional().nullable(),
    scoreLevel: z.number().min(1, "Vui lòng chọn mức điểm").optional().nullable(),
    sCImagoFieldId: z.string().uuid("ID lĩnh vực SCImago không hợp lệ").optional().nullable(),
    fieldId: z.string().uuid("ID lĩnh vực không hợp lệ").optional().nullable(),
  }),
  coAuthorUserIds: z.array(z.string().uuid("ID đồng tác giả không hợp lệ")).optional().default([]),
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
}

export default function WorkForm({
  initialData,
  onSubmit,
  isLoading,
  workTypes,
  workLevels,
  authorRoles,
  purposes,
  scimagoFields,
  fields,
  activeTab,
}: WorkFormProps) {
  const [searchTerm, setSearchTerm] = useState("");
  const [selectedCoAuthors, setSelectedCoAuthors] = useState<User[]>([]);
  const [detailsText, setDetailsText] = useState<string>("");
  const [isLoadingCoAuthors, setIsLoadingCoAuthors] = useState(false);
  
  // Lấy userId từ localStorage hoặc context auth nếu có
  // Trong ứng dụng thực tế, bạn nên sử dụng hook auth chính thức
  const currentUserId = localStorage.getItem("userId") || "current-user-id";

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
    watch,
    getValues,
  } = useForm<WorkFormData>({
    resolver: zodResolver(schema),
    defaultValues: initialData 
      ? {
          ...initialData,
          timePublished: initialData.timePublished ? new Date(initialData.timePublished) : null,
          details: initialData.details || {},
          source: initialData.source,
          author: {
            authorRoleId: initialData.author?.authorRoleId || "",
            purposeId: initialData.author?.purposeId || "",
            position: initialData.author?.position || 1,
            scoreLevel: initialData.author?.scoreLevel,
            sCImagoFieldId: initialData.author?.scImagoFieldId || "",
            fieldId: initialData.author?.fieldId || "",
          },
          coAuthorUserIds: initialData.coAuthorUserIds?.filter(id => id.toString() !== currentUserId) || [],
        }
      : {
          title: "",
          timePublished: null,
          totalAuthors: null,
          totalMainAuthors: null,
          details: {},
          source: WorkSource.NguoiDungKeKhai,
          workTypeId: "",
          workLevelId: "",
          author: {
            authorRoleId: "",
            purposeId: "",
            position: 1,
            scoreLevel: undefined,
            sCImagoFieldId: "",
            fieldId: "",
          },
          coAuthorUserIds: [],
        },
  });

  // Watch workTypeId để lấy dữ liệu phù hợp
  const workTypeId = watch("workTypeId");

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

  const { data: scimagoFieldsData } = useQuery({
    queryKey: ["scimagoFields", workTypeId],
    queryFn: () => getScimagoFieldsByWorkTypeId(workTypeId),
    enabled: !!workTypeId,
  });

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
    return usersData.data.filter(user => user.id !== currentUserId);
  }, [usersData?.data, currentUserId]);

  // Load dữ liệu từ initialData khi nó thay đổi
  useEffect(() => {
    if (initialData) {
      console.log("initialData đã thay đổi:", initialData);
      console.log("coAuthorUserIds:", initialData.coAuthorUserIds);
      
      // Reset form với dữ liệu từ initialData
      reset({
        ...initialData,
        timePublished: initialData.timePublished ? new Date(initialData.timePublished) : null,
        details: initialData.details || {},
        source: initialData.source,
        author: {
          authorRoleId: initialData.author?.authorRoleId || "",
          purposeId: initialData.author?.purposeId || "",
          position: initialData.author?.position || 1,
          scoreLevel: initialData.author?.scoreLevel,
          sCImagoFieldId: initialData.author?.scImagoFieldId || "",
          fieldId: initialData.author?.fieldId || "",
        },
        // Đảm bảo lọc ra người dùng hiện tại
        coAuthorUserIds: initialData.coAuthorUserIds?.filter(id => id.toString() !== currentUserId) || [],
      });

      // Xử lý đồng tác giả
      if (initialData.coAuthorUserIds?.length > 0) {
        // Lọc ra người dùng hiện tại trước khi tải thông tin
        const filteredCoAuthorIds = initialData.coAuthorUserIds
          .filter(id => id.toString() !== currentUserId);
        
        // Tải thông tin đồng tác giả nếu có danh sách ID đã lọc
        if (filteredCoAuthorIds.length > 0) {
          setIsLoadingCoAuthors(true);
          console.log("Bắt đầu tải thông tin đồng tác giả cho các id đã lọc:", filteredCoAuthorIds);
          
          const fetchCoAuthors = async () => {
            try {
              // Lấy thông tin chi tiết của từng đồng tác giả
              const userPromises = filteredCoAuthorIds.map(async (id) => {
                try {
                  console.log("Đang lấy thông tin user với ID:", id);
                  const response = await getUserById(id.toString());
                  console.log("Kết quả lấy thông tin user:", response);
                  return response.data;
                } catch (error) {
                  console.error("Lỗi khi lấy thông tin user:", id, error);
                  return null;
                }
              });
              
              const users = await Promise.all(userPromises);
              const validUsers = users.filter((user): user is User => user !== null);
              console.log("Đã tải được thông tin đồng tác giả:", validUsers);
              
              // Đảm bảo lọc ra người dùng hiện tại một lần nữa để tránh trường hợp ID bị so sánh khác kiểu dữ liệu
              const finalFilteredUsers = validUsers.filter(user => user.id !== currentUserId);
              setSelectedCoAuthors(finalFilteredUsers);
            } catch (error) {
              console.error("Lỗi khi tải thông tin đồng tác giả:", error);
            } finally {
              setIsLoadingCoAuthors(false);
            }
          };
          
          fetchCoAuthors();
        } else {
          setSelectedCoAuthors([]);
        }
      } else {
        setSelectedCoAuthors([]);
      }
    }
  }, [initialData, reset, currentUserId]);

  // Reset các trường phụ thuộc khi workTypeId thay đổi và không có initialData
  useEffect(() => {
    if (workTypeId && !initialData) {
      setValue("workLevelId", "");
      setValue("author.authorRoleId", "");
      setValue("author.purposeId", "");
      setValue("author.sCImagoFieldId", "");
    }
  }, [workTypeId, setValue, initialData]);

  // Cập nhật coAuthorUserIds khi selectedCoAuthors thay đổi
  useEffect(() => {
    console.log("Cập nhật coAuthorUserIds với:", selectedCoAuthors);
    setValue("coAuthorUserIds", selectedCoAuthors.map(user => user.id));
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
        // Convert details object to formatted JSON string for display
        const detailsJson = JSON.stringify(initialData.details, null, 2);
        setDetailsText(detailsJson);
      } catch (error) {
        console.error("Lỗi khi parse details:", error);
        setDetailsText("");
      }
    } else {
      setDetailsText("");
    }
  }, [initialData]);

  // Xử lý khi người dùng nhập chi tiết
  const handleDetailsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const value = e.target.value;
    setDetailsText(value);
    
    try {
      // Cố gắng parse giá trị thành object JSON
      const detailsObj = value ? JSON.parse(value) : {};
      setValue("details", detailsObj);
    } catch (error) {
      // Nếu không parse được, vẫn lưu giá trị text nhưng không update form
      console.error("Lỗi khi parse JSON:", error);
    }
  };

  // Xử lý khi đồng tác giả thay đổi
  const handleCoAuthorChange = (_: any, newValue: User[]) => {
    console.log("Đã chọn đồng tác giả:", newValue);
    
    // Đảm bảo không có người dùng hiện tại trong danh sách đồng tác giả
    const filteredCoAuthors = newValue.filter(user => user.id !== currentUserId);
    
    setSelectedCoAuthors(filteredCoAuthors);
    
    // Cập nhật giá trị trong form
    setValue(
      "coAuthorUserIds",
      filteredCoAuthors.map((user) => user.id),
      { shouldValidate: true }
    );
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Grid container spacing={2}>
        {activeTab === 0 && (
          // Tab thông tin công trình
          <>
            <Grid item xs={12}>
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
                  />
                )}
              />
            </Grid>

            <Grid item xs={12}>
                <Controller
                  name="timePublished"
                  control={control}
                  render={({ field }) => (
                  <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={vi}>
                    <DatePicker
                      label="Thời gian xuất bản"
                      value={field.value ? new Date(field.value) : null}
                      onChange={(date) => field.onChange(date)}
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
                  />
                )}
              />
            </Grid>

            <Grid item xs={12}>
              <TextField
                label="Chi tiết công trình (định dạng JSON)"
                multiline
                rows={4}
                fullWidth
                value={detailsText}
                onChange={handleDetailsChange}
                error={!!errors.details}
                helperText={errors.details?.message?.toString() || "Nhập chi tiết công trình dưới dạng JSON, ví dụ: { \"ISSN\": \"1234-5678\", \"DOI\": \"10.1234/abcd\" }"}
                placeholder='{ "ISSN": "1234-5678", "DOI": "10.1234/abcd" }'
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
                    label="Cấp độ công trình"
                    fullWidth
                    error={!!errors.workLevelId}
                    helperText={errors.workLevelId?.message?.toString()}
                    disabled={!workTypeId}
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

            <Grid item xs={12}>
              <Controller
                name="coAuthorUserIds"
                control={control}
                render={({ field }) => (
                  <Autocomplete
                    multiple
                    id="coAuthorUserIds"
                    options={filteredUsers}
                    getOptionLabel={(option) => {
                      if (typeof option === 'string') return option;
                      return `${option.fullName} - ${option.userName}${option.departmentName ? ` - ${option.departmentName}` : ''}`;
                    }}
                    isOptionEqualToValue={(option, value) => option.id === value.id}
                    value={selectedCoAuthors}
                    onChange={handleCoAuthorChange}
                    loading={isLoadingUsers || isLoadingCoAuthors}
                    renderTags={(value, getTagProps) =>
                      value.map((option, index) => {
                        const props = getTagProps({ index });
                        return (
                          <Chip
                            {...props}
                            label={`${option.fullName} - ${option.userName}${option.departmentName ? ` - ${option.departmentName}` : ''}`}
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
                        helperText={errors.coAuthorUserIds?.message || "Thêm các đồng tác giả (không bao gồm bạn)"}
                        InputProps={{
                          ...params.InputProps,
                          endAdornment: (
                            <>
                              {(isLoadingUsers || isLoadingCoAuthors) ? <CircularProgress color="inherit" size={20} /> : null}
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
          </>
        )}

        {activeTab === 1 && (
          // Tab thông tin tác giả
          <>
            <Grid item xs={6}>
                <Controller
                name="author.authorRoleId"
                  control={control}
                  render={({ field }) => (
                  <TextField
                      {...field}
                    select
                    label="Vai trò tác giả"
                    fullWidth
                    error={!!errors.author?.authorRoleId}
                    helperText={errors.author?.authorRoleId?.message?.toString()}
                    disabled={!workTypeId}
                  >
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
                    label="Mục đích"
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
                  render={({ field }) => (
                  <TextField
                      {...field}
                    label="Vị trí tác giả"
                    type="number"
                    fullWidth
                    error={!!errors.author?.position}
                    helperText={errors.author?.position?.message?.toString()}
                  />
                )}
              />
            </Grid>

            <Grid item xs={6}>
                <Controller
                name="author.scoreLevel"
                  control={control}
                  render={({ field }) => (
                  <TextField
                      {...field}
                    select
                    label="Mức điểm"
                    fullWidth
                    error={!!errors.author?.scoreLevel}
                    helperText={errors.author?.scoreLevel?.message?.toString()}
                  >
                    <MenuItem value={ScoreLevel.One}>1 điểm</MenuItem>
                    <MenuItem value={ScoreLevel.ZeroPointSevenFive}>0.75 điểm</MenuItem>
                    <MenuItem value={ScoreLevel.ZeroPointFive}>0.5 điểm</MenuItem>
                    <MenuItem value={ScoreLevel.TenPercent}>Top 10%</MenuItem>
                    <MenuItem value={ScoreLevel.ThirtyPercent}>Top 30%</MenuItem>
                    <MenuItem value={ScoreLevel.FiftyPercent}>Top 50%</MenuItem>
                    <MenuItem value={ScoreLevel.HundredPercent}>Top 100%</MenuItem>
                  </TextField>
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="author.sCImagoFieldId"
                control={control}
                render={({ field }) => (
              <TextField
                    {...field}
                    select
                    label="Ngành SCImago"
                fullWidth
                    error={!!errors.author?.sCImagoFieldId}
                    helperText={errors.author?.sCImagoFieldId?.message?.toString()}
                    disabled={!workTypeId}
                  >
                    {scimagoFieldsData?.data?.map((field) => (
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
                  render={({ field }) => (
                  <TextField
                      {...field}
                    select
                    label="Ngành tính điểm"
                    fullWidth
                    error={!!errors.author?.fieldId}
                    helperText={errors.author?.fieldId?.message?.toString()}
                  >
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
            {isLoading ? <CircularProgress size={24} /> : (initialData ? "Cập nhật" : "Thêm mới")}
        </Button>
        </Grid>
      </Grid>
    </form>
  );
} 