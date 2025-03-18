import { useEffect, useState } from 'react';
import { Grid, TextField, Button, MenuItem, Box } from '@mui/material';
import { useForm, Controller } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import CoAuthorSelect from './CoAuthorSelect';
import { WorkSource } from '../lib/types/enums/WorkSource';
import { Work } from '../lib/types/models/Work';
import { DatePicker } from '@mui/x-date-pickers';
import dayjs from 'dayjs';

interface WorkFormProps {
  initialData?: any;
  onSubmit: (data: FormData) => void;
  isLoading?: boolean;
  workTypes: Array<{ id: string; name: string }>;
  workLevels: Array<{ id: string; name: string }>;
  authorRoles: Array<{ id: string; name: string }>;
  purposes: Array<{ id: string; name: string }>;
  scimagoFields: Array<{ id: string; name: string }>;
  fields: Array<{ id: string; name: string }>;
}

const schema = z.object({
  title: z.string().min(1, 'Tiêu đề là bắt buộc'),
  timePublished: z.string().min(1, 'Thời gian xuất bản là bắt buộc'),
  totalAuthors: z.number().min(1, 'Số tác giả phải lớn hơn 0'),
  totalMainAuthors: z.number().min(1, 'Số tác giả chính phải lớn hơn 0'),
  details: z.string().optional(),
  source: z.number(),
  workTypeId: z.string().uuid('Loại công trình không hợp lệ'),
  workLevelId: z.string().uuid('Cấp công trình không hợp lệ'),
  coAuthorUserIds: z.array(z.string().uuid('ID đồng tác giả không hợp lệ')),
  author: z.object({
    authorRoleId: z.string().uuid('Vai trò tác giả là bắt buộc'),
    purposeId: z.string().uuid('Mục đích là bắt buộc'),
    position: z.number().min(1, 'Vị trí tác giả phải lớn hơn 0'),
    scoreLevel: z.number().min(0, 'Mức điểm không hợp lệ'),
    scimagoFieldId: z.string().uuid('Lĩnh vực Scimago là bắt buộc'),
    fieldId: z.string().uuid('Lĩnh vực là bắt buộc'),
  }),
});

type FormData = z.infer<typeof schema>;

const defaultValues: FormData = {
  title: '',
  timePublished: '',
  totalAuthors: 1,
  totalMainAuthors: 1,
  details: '',
  source: 0,
  workTypeId: '',
  workLevelId: '',
  coAuthorUserIds: [],
  author: {
    authorRoleId: '',
    purposeId: '',
    position: 1,
    scoreLevel: 0,
    scimagoFieldId: '',
    fieldId: '',
  },
};

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
}: WorkFormProps) {
  const {
    control,
    handleSubmit,
    formState: { errors },
  } = useForm<FormData>({
    resolver: zodResolver(schema),
    defaultValues: initialData || defaultValues,
  });

  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <Grid container spacing={2}>
        {/* Thông tin công trình */}
        <Grid item xs={12}>
          <Controller
            name="title"
            control={control}
            render={({ field }) => (
              <TextField
                {...field}
                label="Tiêu đề"
                fullWidth
                error={!!errors.title}
                helperText={errors.title?.message}
              />
            )}
          />
        </Grid>

        <Grid item xs={12}>
          <Controller
            name="timePublished"
            control={control}
            render={({ field }) => (
              <DatePicker
                label="Thời gian xuất bản"
                value={field.value ? dayjs(field.value) : null}
                onChange={(date) => field.onChange(date?.toISOString())}
                slotProps={{
                  textField: {
                    fullWidth: true,
                    error: !!errors.timePublished,
                    helperText: errors.timePublished?.message,
                  },
                }}
              />
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
                helperText={errors.totalAuthors?.message}
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
                helperText={errors.totalMainAuthors?.message}
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
                label="Chi tiết"
                multiline
                rows={4}
                fullWidth
                error={!!errors.details}
                helperText={errors.details?.message}
              />
            )}
          />
        </Grid>

        <Grid item xs={12}>
          <Controller
            name="source"
            control={control}
            render={({ field }) => (
              <TextField
                {...field}
                select
                label="Nguồn"
                fullWidth
                error={!!errors.source}
                helperText={errors.source?.message}
              >
                <MenuItem value={WorkSource.NguoiDungKeKhai}>Người dùng kê khai</MenuItem>
                <MenuItem value={WorkSource.WoS}>WoS</MenuItem>
                <MenuItem value={WorkSource.Scopus}>Scopus</MenuItem>
              </TextField>
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
                helperText={errors.workTypeId?.message}
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
                helperText={errors.workLevelId?.message}
              >
                {workLevels.map((level) => (
                  <MenuItem key={level.id} value={level.id}>
                    {level.name}
                  </MenuItem>
                ))}
              </TextField>
            )}
          />
        </Grid>

        {/* Thông tin tác giả */}
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
                helperText={errors.author?.authorRoleId?.message}
              >
                {authorRoles.map((role) => (
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
                helperText={errors.author?.purposeId?.message}
              >
                {purposes.map((purpose) => (
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
                helperText={errors.author?.position?.message}
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
                label="Mức điểm"
                type="number"
                fullWidth
                error={!!errors.author?.scoreLevel}
                helperText={errors.author?.scoreLevel?.message}
              />
            )}
          />
        </Grid>

        <Grid item xs={6}>
          <Controller
            name="author.scimagoFieldId"
            control={control}
            render={({ field }) => (
              <TextField
                {...field}
                select
                label="Lĩnh vực Scimago"
                fullWidth
                error={!!errors.author?.scimagoFieldId}
                helperText={errors.author?.scimagoFieldId?.message}
              >
                {scimagoFields.map((field) => (
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
                label="Lĩnh vực"
                fullWidth
                error={!!errors.author?.fieldId}
                helperText={errors.author?.fieldId?.message}
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

        {/* Đồng tác giả */}
        <Grid item xs={12}>
          <Controller
            name="coAuthorUserIds"
            control={control}
            render={({ field }) => (
              <CoAuthorSelect
                value={field.value}
                onChange={field.onChange}
                error={!!errors.coAuthorUserIds}
                helperText={errors.coAuthorUserIds?.message}
              />
            )}
          />
        </Grid>

        <Grid item xs={12}>
          <Button
            type="submit"
            variant="contained"
            color="primary"
            fullWidth
            disabled={isLoading}
          >
            {isLoading ? 'Đang xử lý...' : 'Lưu'}
          </Button>
        </Grid>
      </Grid>
    </form>
  );
} 