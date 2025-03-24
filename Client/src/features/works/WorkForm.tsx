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
  Checkbox,
  Typography
} from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { vi } from "date-fns/locale";
import { useEffect, useState, useMemo } from "react";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";
import { Work } from "../../lib/types/models/Work";
import { WorkSource } from "../../lib/types/enums/WorkSource";
import { ScoreLevel } from "../../lib/types/enums/ScoreLevel";
import { getWorkLevelsByWorkTypeId } from "../../lib/api/workLevelsApi";
import { getAuthorRolesByWorkTypeId } from "../../lib/api/authorRolesApi";
import { getPurposesByWorkTypeId } from "../../lib/api/purposesApi";
import { getScimagoFieldsByWorkTypeId } from "../../lib/api/scimagoFieldsApi";
import { searchUsers, getUserById } from "../../lib/api/usersApi";
import { User } from "../../lib/types/models/User";
import { useQuery } from "@tanstack/react-query";
import { getScoreLevelsByFilters } from "../../lib/api/scoreLevelsApi";

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
  coAuthorUserIds: z.array(z.string().uuid("ID đồng tác giả không hợp lệ")).optional().default([]),
  author: z.object({
    authorRoleId: z.string().uuid("ID vai trò tác giả không hợp lệ").optional().nullable(),
    purposeId: z.string().uuid("ID mục đích không hợp lệ"),
    position: z.number().min(1, "Vị trí tác giả phải lớn hơn 0").optional().nullable(),
    scoreLevel: z.number().min(0, "Mức điểm không hợp lệ").optional().nullable(),
    sCImagoFieldId: z.string().uuid("ID lĩnh vực SCImago không hợp lệ").optional().nullable(),
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
  const [detailsFields, setDetailsFields] = useState<Record<string, string>>({});
  const [isLoadingCoAuthors, setIsLoadingCoAuthors] = useState(false);
  const [visibleScoreLevels, setVisibleScoreLevels] = useState<number[]>([]);
  
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
          coAuthorUserIds: initialData.coAuthorUserIds?.filter(id => id.toString() !== currentUserId) || [],
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
  const getDetailsFieldsConfig = () => {
    if (!workTypeId) return [];

    // Bài báo khoa học
    if (workTypeId === "2732c858-77dc-471d-bd9a-464a3142530a") {
      // Cấp WoS
      if (workLevelId === "0b031a2d-4ac5-48fb-9759-f7a2fe2f7290") {
        return [
          { key: "Tên tạp chí khoa học", label: "Tên tạp chí khoa học", required: true },
          { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
          { key: "Số trang", label: "Số trang", required: false },
          { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
          { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
          { key: "Loại WoS", label: "Loại WoS", required: true },
          { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: false },
        ];
      }
      // Cấp Scopus
      else if (workLevelId === "34f94668-7151-457d-aa06-4bf4e2b27df3") {
        return [
          { key: "Tên tạp chí khoa học", label: "Tên tạp chí khoa học", required: true },
          { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
          { key: "Số trang", label: "Số trang", required: false },
          { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
          { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
          { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: false },
        ];
      }
      // Các cấp khác: Trường, bộ/ngành, quốc tế
      else {
        return [
          { key: "Tên tạp chí", label: "Tên tạp chí khoa học", required: true },
          { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
          { key: "Số trang", label: "Số trang", required: false },
          { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
          { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
        ];
      }
    }
    // Báo cáo khoa học
    else if (workTypeId === "03412ca7-8ccf-4903-9018-457768060ab4") {
      // Cấp WoS
      if (workLevelId === "f81c134b-fd83-4e25-9590-cf7ecfc5b203") {
        return [
          { key: "Tên tạp chí khoa học", label: "Tên tạp chí khoa học", required: true },
          { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
          { key: "Số trang", label: "Số trang", required: false },
          { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
          { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
          { key: "Loại WoS", label: "Loại WoS", required: true },
          { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: false },
        ];
      }
      // Cấp Scopus
      else if (workLevelId === "f0dcb91e-04b1-46c5-a05d-bbcaf7ef89f9") {
        return [
          { key: "Tên tạp chí khoa học", label: "Tên tạp chí khoa học", required: true },
          { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
          { key: "Số trang", label: "Số trang", required: false },
          { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
          { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
          { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: false },
        ];
      }
      // Các cấp khác: Trường, bộ/ngành, quốc tế, quốc gia
      else {
        return [
          { key: "Tên tạp chí", label: "Tên tạp chí khoa học", required: true },
          { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
          { key: "Số trang", label: "Số trang", required: false },
          { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
          { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
        ];
      }
    }
    // Chương sách, chuyên khảo, giáo trình - sách, tài liệu hướng dẫn, tham khảo
    else if (["3bbfc66a-3144-4edf-959b-e049d7e33d97", "61bbbecc-038a-43b7-aafa-a95e25a93f38", 
              "628a119e-324f-42b8-8ff4-e29ee5c643a9", "84a14a8b-eae8-4720-bc7c-e1f93b35a256", 
              "8aaf0a8a-35ed-4768-8fd4-44fc4a561cd0", "1ff8d087-e0c3-45df-befc-662c0a80c10c"].includes(workTypeId)) {
      return [
        { key: "Tên tạp chí khoa học", label: "Tên tạp chí khoa học", required: true },
        { key: "Tập, số phát hành", label: "Tập, số phát hành", required: false },
        { key: "Số trang", label: "Số trang", required: false },
        { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: false },
        { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: false },
      ];
    }
    // Đề tài, giáo trình
    else if (["49cf7589-fb84-4934-be8e-991c6319a348", "323371c0-26c7-4549-90f2-11c881be402d"].includes(workTypeId)) {
      return [
        { key: "Mã số đề tài", label: "Mã số đề tài", required: true },
        { key: "Sản phẩm thuộc đề tài", label: "Sản phẩm thuộc đề tài", required: false },
        { key: "Xếp loại", label: "Xếp loại", required: false },
      ];
    }
    // Hội thảo, hội nghị và hướng dẫn SV NCKH
    else if (["140a3e34-ded1-4bfa-8633-fbea545cbdaa", "e2f7974c-47c3-478e-9b53-74093f6c621f"].includes(workTypeId)) {
      return [
        { key: "Hoạt động", label: "Hoạt động", required: true },
      ];
    }
    
    // Mặc định trả về mảng rỗng
    return [];
  };

  
  const detailsConfig = getDetailsFieldsConfig();

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
        coAuthorUserIds: initialData.coAuthorUserIds?.filter(id => id.toString() !== currentUserId) || [],
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
        if (initialData.coAuthorUserIds && initialData.coAuthorUserIds.length > 0) {
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
          purposeId || undefined
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
    const coAuthorIds = selectedCoAuthors.map(user => user.id);
    console.log("Danh sách ID đồng tác giả cập nhật:", coAuthorIds);
    
    // Đảm bảo cập nhật giá trị vào form
    setValue(
      "coAuthorUserIds",
      coAuthorIds,
      { shouldValidate: true }
    );
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
  const handleDetailsChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    try {
      const jsonValue = JSON.parse(e.target.value);
      setDetailsFields(jsonValue);
      setValue("details", jsonValue);
    } catch (error) {
      console.error("Lỗi khi parse JSON:", error);
    }
  };

  // Xử lý khi đồng tác giả thay đổi
  const handleCoAuthorChange = (_: any, newValue: User[]) => {
    console.log("Đã chọn đồng tác giả:", newValue);
    
    // Đảm bảo không có người dùng hiện tại trong danh sách đồng tác giả
    const filteredCoAuthors = newValue.filter(user => user.id !== currentUserId);
    
    setSelectedCoAuthors(filteredCoAuthors);
    
    // Cập nhật giá trị trong form - chỉ lưu trữ IDs
    const coAuthorIds = filteredCoAuthors.map(user => user.id);
    console.log("Danh sách ID đồng tác giả cập nhật:", coAuthorIds);
    
    setValue(
      "coAuthorUserIds",
      coAuthorIds,
      { shouldValidate: true }
    );
  };

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
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
                  />
                )}
              />
            </Grid>

            <Grid item xs={6}>
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
                    label="Cấp công trình"
                    fullWidth
                    error={!!errors.workLevelId}
                    helperText={errors.workLevelId?.message?.toString()}
                    disabled={!workTypeId || !hasWorkLevels}
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

            {/* Chi tiết động dựa trên loại công trình và cấp công trình */}
            {detailsConfig.length > 0 && (
              <Grid item xs={12}>
                <Grid container spacing={2}>
                  {detailsConfig.map(field => (
                    <Grid item xs={12} sm={6} key={field.key}>
                      <TextField
                        label={field.label}
                        fullWidth
                        value={detailsFields[field.key] || ""}
                        onChange={(e) => handleDetailsFieldChange(field.key, e.target.value)}
                        required={field.required}
                        error={field.required && (!detailsFields[field.key] || detailsFields[field.key].trim() === "")}
                        helperText={field.required && (!detailsFields[field.key] || detailsFields[field.key].trim() === "") 
                          ? "Trường này là bắt buộc" 
                          : ""}
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
                      const newValue = e.target.value === "" ? null : e.target.value;
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
                      const newValue = e.target.value === "" ? null : Number(e.target.value);
                      onChange(newValue);
                    }}
                  >
                    <MenuItem value="">Chọn mức điểm</MenuItem>
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoMotDiem) && (
                      <MenuItem value={ScoreLevel.BaiBaoMotDiem}>Bài báo 1 điểm</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoKhongBayNamDiem) && (
                      <MenuItem value={ScoreLevel.BaiBaoKhongBayNamDiem}>Bài báo 0.75 điểm</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoNuaDiem) && (
                      <MenuItem value={ScoreLevel.BaiBaoNuaDiem}>Bài báo 0.5 điểm</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoTopMuoi) && (
                      <MenuItem value={ScoreLevel.BaiBaoTopMuoi}>Bài báo khoa học thuộc top 10% tạp chí hàng đầu</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoTopBaMuoi) && (
                      <MenuItem value={ScoreLevel.BaiBaoTopBaMuoi}>Bài báo khoa học thuộc top 30% tạp chí hàng đầu</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoTopNamMuoi) && (
                      <MenuItem value={ScoreLevel.BaiBaoTopNamMuoi}>Bài báo khoa học thuộc top 50% tạp chí hàng đầu</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.BaiBaoTopConLai) && (
                      <MenuItem value={ScoreLevel.BaiBaoTopConLai}>Bài báo khoa học thuộc top còn lại tạp chí hàng đầu</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.HDSVDatGiaiKK) && (
                      <MenuItem value={ScoreLevel.HDSVDatGiaiKK}>Hướng dẫn đề tài NCKH đạt giải KK</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.HDSVDatGiaiBa) && (
                      <MenuItem value={ScoreLevel.HDSVDatGiaiBa}>Hướng dẫn đề tài NCKH đạt giải Ba</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.HDSVDatGiaiNhi) && (
                      <MenuItem value={ScoreLevel.HDSVDatGiaiNhi}>Hướng dẫn đề tài NCKH đạt giải Nhì</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.HDSVDatGiaiNhat) && (
                      <MenuItem value={ScoreLevel.HDSVDatGiaiNhat}>Hướng dẫn đề tài NCKH đạt giải Nhất</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.HDSVConLai) && (
                      <MenuItem value={ScoreLevel.HDSVConLai}>HDSV còn lại</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.TacPhamNgheThuatCapTruong) && (
                      <MenuItem value={ScoreLevel.TacPhamNgheThuatCapTruong}>Tác phẩm nghệ thuật cấp trường</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.TacPhamNgheThuatCapTinhThanhPho) && (
                      <MenuItem value={ScoreLevel.TacPhamNgheThuatCapTinhThanhPho}>Tác phẩm nghệ thuật cấp tỉnh/thành phố</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.TacPhamNgheThuatCapQuocGia) && (
                      <MenuItem value={ScoreLevel.TacPhamNgheThuatCapQuocGia}>Tác phẩm nghệ thuật cấp quốc gia</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.TacPhamNgheThuatCapQuocTe) && (
                      <MenuItem value={ScoreLevel.TacPhamNgheThuatCapQuocTe}>Tác phẩm nghệ thuật cấp quốc tế</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.ThanhTichHuanLuyenCapQuocGia) && (
                      <MenuItem value={ScoreLevel.ThanhTichHuanLuyenCapQuocGia}>Thành tích huấn luyện cấp quốc gia</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.ThanhTichHuanLuyenCapQuocTe) && (
                      <MenuItem value={ScoreLevel.ThanhTichHuanLuyenCapQuocTe}>Thành tích huấn luyện cấp quốc tế</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho) && (
                      <MenuItem value={ScoreLevel.GiaiPhapHuuIchCapTinhThanhPho}>Giải pháp hữu ích cấp Tỉnh/Thành phố</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.GiaiPhapHuuIchCapQuocGia) && (
                      <MenuItem value={ScoreLevel.GiaiPhapHuuIchCapQuocGia}>Giải pháp hữu ích cấp cấp Quốc gia</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.GiaiPhapHuuIchCapQuocTe) && (
                      <MenuItem value={ScoreLevel.GiaiPhapHuuIchCapQuocTe}>Giải pháp hữu ích cấp Quốc tế</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.KetQuaNghienCuu) && (
                      <MenuItem value={ScoreLevel.KetQuaNghienCuu}>Kết quả nghiên cứu</MenuItem>
                    )}
                    {visibleScoreLevels.includes(ScoreLevel.Sach) && (
                      <MenuItem value={ScoreLevel.Sach}>Sách</MenuItem>
                    )}
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
                      const newValue = e.target.value === "" ? null : e.target.value;
                      onChange(newValue);
                    }}
                  >
                    <MenuItem value="">Chọn ngành SCImago</MenuItem>
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
                      const newValue = e.target.value === "" ? null : e.target.value;
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
            {isLoading ? <CircularProgress size={24} /> : (initialData ? "Cập nhật" : "Thêm mới")}
        </Button>
        </Grid>
      </Grid>
    </form>
  );
} 