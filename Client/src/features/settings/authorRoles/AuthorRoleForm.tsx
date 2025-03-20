import { zodResolver } from "@hookform/resolvers/zod";
import {
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  TextField,
  MenuItem,
  CircularProgress,
  FormControlLabel,
  Checkbox,
  Grid,
} from "@mui/material";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect } from "react";
import { useForm, Controller } from "react-hook-form";
import { toast } from "react-toastify";
import * as z from "zod";
import { createAuthorRole, updateAuthorRole } from "../../../lib/api/authorRolesApi";
import { getWorkTypes } from "../../../lib/api/workTypesApi";
import { AuthorRole } from "../../../lib/types/models/AuthorRole";

// Define validation schema
const schema = z.object({
  name: z.string().min(2, "Tên vai trò tác giả phải có ít nhất 2 ký tự"),
  workTypeId: z.string().uuid("Vui lòng chọn loại công trình"),
  isMainAuthor: z.boolean(),
});

type AuthorRoleFormData = z.infer<typeof schema>;

interface AuthorRoleFormProps {
  open: boolean;
  handleClose: () => void;
  data?: AuthorRole | null;
}

export default function AuthorRoleForm({ open, handleClose, data }: AuthorRoleFormProps) {
  const queryClient = useQueryClient();

  // Fetch work types
  const { data: workTypesData, isLoading: isLoadingWorkTypes } = useQuery({
    queryKey: ["workTypes"],
    queryFn: getWorkTypes,
  });

  const {
    control,
    handleSubmit,
    setValue,
    reset,
    formState: { errors },
  } = useForm<AuthorRoleFormData>({
    resolver: zodResolver(schema),
    defaultValues: {
      name: "",
      workTypeId: "",
      isMainAuthor: false,
    },
  });

  useEffect(() => {
    if (data) {
      setValue("name", data.name);
      setValue("workTypeId", data.workTypeId);
      setValue("isMainAuthor", data.isMainAuthor);
    } else {
      reset();
    }
  }, [data, setValue, reset]);

  // Mutation for create and update operations
  const mutation = useMutation({
    mutationFn: async (formData: AuthorRoleFormData) => {
      if (data?.id) {
        return updateAuthorRole(data.id, formData);
      } else {
        return createAuthorRole(formData);
      }
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["authorRoles"] });
      toast.success(data?.id ? "Vai trò tác giả đã được cập nhật" : "Vai trò tác giả đã được thêm");
      handleClose();
    },
    onError: (error) => {
      console.error("API Error:", error);
      toast.error("Đã xảy ra lỗi: " + (error as Error).message);
    },
  });

  // Handle form submission
  const onSubmit = async (formData: AuthorRoleFormData) => {
    await mutation.mutateAsync(formData);
  };

  return (
    <Dialog open={open} onClose={handleClose} maxWidth="sm" fullWidth>
      <DialogTitle>{data ? "Cập nhật Vai trò tác giả" : "Thêm Vai trò tác giả"}</DialogTitle>
      <DialogContent>
        <Grid container spacing={2} sx={{ mt: 1 }}>
          <Grid item xs={12}>
            <Controller
              name="name"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  label="Tên vai trò tác giả"
                  error={!!errors.name}
                  helperText={errors.name?.message}
                  fullWidth
                  disabled={mutation.isPending}
                />
              )}
            />
          </Grid>

          <Grid item xs={12}>
            <Controller
              name="workTypeId"
              control={control}
              render={({ field }) => (
                <TextField
                  {...field}
                  select
                  label="Loại công trình"
                  error={!!errors.workTypeId}
                  helperText={errors.workTypeId?.message}
                  fullWidth
                  disabled={mutation.isPending || isLoadingWorkTypes}
                >
                  {isLoadingWorkTypes ? (
                    <MenuItem disabled>Đang tải...</MenuItem>
                  ) : (
                    workTypesData?.data?.map((workType) => (
                      <MenuItem key={workType.id} value={workType.id}>
                        {workType.name}
                      </MenuItem>
                    ))
                  )}
                </TextField>
              )}
            />
          </Grid>

          <Grid item xs={12}>
            <Controller
              name="isMainAuthor"
              control={control}
              render={({ field }) => (
                <FormControlLabel
                  control={
                    <Checkbox
                      checked={field.value}
                      onChange={field.onChange}
                      disabled={mutation.isPending}
                    />
                  }
                  label="Là tác giả chính"
                />
              )}
            />
          </Grid>
        </Grid>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose} disabled={mutation.isPending}>
          Hủy
        </Button>
        <Button
          onClick={handleSubmit(onSubmit)}
          variant="contained"
          color="primary"
          disabled={mutation.isPending || isLoadingWorkTypes}
        >
          {mutation.isPending ? <CircularProgress size={24} /> : (data ? "Cập nhật" : "Thêm mới")}
        </Button>
      </DialogActions>
    </Dialog>
  );
} 