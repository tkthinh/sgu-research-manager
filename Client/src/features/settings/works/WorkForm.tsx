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
  Stack
} from "@mui/material";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { AdapterDateFns } from "@mui/x-date-pickers/AdapterDateFns";
import { vi } from "date-fns/locale";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import * as z from "zod";
import { Work } from "../../../lib/types/models/Work";
import { WorkSource } from "../../../lib/types/enums/WorkSource";
import { ScoreLevel } from "../../../lib/types/enums/ScoreLevel";
import { getWorkLevelsByWorkTypeId } from "../../../lib/api/workLevelsApi";
import { getAuthorRolesByWorkTypeId } from "../../../lib/api/authorRolesApi";
import { getPurposesByWorkTypeId } from "../../../lib/api/purposesApi";
import { getScimagoFieldsByWorkTypeId } from "../../../lib/api/scimagoFieldsApi";
import { searchUsers } from "../../../lib/api/usersApi";
import { User } from "../../../lib/types/models/User";
import { useQuery } from "@tanstack/react-query";

// Define validation schema
const schema = z.object({
  title: z.string().min(2, "Tên công trình phải có ít nhất 2 ký tự"),
  timePublished: z.any().optional().nullable(),
  totalAuthors: z.number().min(1, "Số tác giả phải lớn hơn 0").optional().nullable(),
  totalMainAuthors: z.number().min(1, "Số tác giả chính phải lớn hơn 0").optional().nullable(),
  details: z.any().optional(),
  source: z.number(),
  workTypeId: z.string().min(1, "Vui lòng chọn loại công trình"),
  workLevelId: z.string().optional().nullable(),
  author: z.object({
  authorRoleId: z.string().min(1, "Vui lòng chọn vai trò tác giả"),
  purposeId: z.string().min(1, "Vui lòng chọn mục đích"),
    position: z.number().min(1, "Vị trí tác giả phải lớn hơn 0").optional().nullable(),
    scoreLevel: z.number().min(1, "Vui lòng chọn mức điểm").optional().nullable(),
    sCImagoFieldId: z.string().optional().nullable(),
    fieldId: z.string().optional().nullable(),
  }),
  coAuthorUserIds: z.array(z.string()).optional().default([]),
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

  const {
    control,
    handleSubmit,
    formState: { errors },
    reset,
    setValue,
    watch,
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
          coAuthorUserIds: initialData.coAuthorUserIds || [],
        }
      : {
          title: "",
          timePublished: null,
          totalAuthors: 1,
          totalMainAuthors: 1,
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

  // Fetch danh sách user khi searchTerm thay đổi
  const { data: usersData } = useQuery({
    queryKey: ["users", searchTerm],
    queryFn: () => searchUsers(searchTerm),
    enabled: searchTerm.length > 0,
    staleTime: 300,
  });

  // Load dữ liệu ban đầu và thông tin đồng tác giả
  useEffect(() => {
    if (initialData) {
      // Reset form với dữ liệu ban đầu
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
        coAuthorUserIds: initialData.coAuthorUserIds || [],
      });

      // Load thông tin đồng tác giả
      if (initialData.coAuthorUserIds?.length > 0) {
        Promise.all(
          initialData.coAuthorUserIds.map(async (id) => {
            const response = await searchUsers(id);
            return response.data[0];
          })
        ).then((users) => {
          const validUsers = users.filter((user): user is User => user !== undefined);
          setSelectedCoAuthors(validUsers);
        });
      }
    }
  }, [initialData, reset]);

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
    setValue("coAuthorUserIds", selectedCoAuthors.map(user => user.id));
  }, [selectedCoAuthors, setValue]);

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
                  render={({ field }) => (
                  <TextField
                      {...field}
                    label="Tổng số tác giả"
                    type="number"
                    fullWidth
                    error={!!errors.totalAuthors}
                    helperText={errors.totalAuthors?.message?.toString()}
                  />
                )}
              />
            </Grid>

            <Grid item xs={6}>
              <Controller
                name="totalMainAuthors"
                control={control}
                render={({ field }) => (
              <TextField
                    {...field}
                    label="Số tác giả chính"
                type="number"
                fullWidth
                    error={!!errors.totalMainAuthors}
                    helperText={errors.totalMainAuthors?.message?.toString()}
                  />
                )}
              />
            </Grid>

            <Grid item xs={12}>
              <Controller
                name="details"
                control={control}
                render={({ field }) => (
              <TextField
                    {...field}
                    label="Chi tiết công trình"
                    multiline
                    rows={3}
                fullWidth
                    error={!!errors.details}
                    helperText={errors.details?.message?.toString()}
                  />
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
              <Autocomplete
                multiple
                options={usersData?.data || []}
                value={selectedCoAuthors}
                onChange={(_, newValue) => setSelectedCoAuthors(newValue)}
                getOptionLabel={(option) => `${option.fullName} - ${option.userName} - ${option.departmentName}`}
                renderInput={(params) => (
                  <TextField
                    {...params}
                    label="Tìm kiếm đồng tác giả"
                    onChange={(e) => setSearchTerm(e.target.value)}
                    fullWidth
                  />
                )}
                renderTags={(value, getTagProps) =>
                  value.map((option, index) => {
                    const props = getTagProps({ index });
                    return (
                      <Chip
                        {...props}
                        label={`${option.fullName} - ${option.userName} - ${option.departmentName}`}
                      />
                    );
                  })
                }
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