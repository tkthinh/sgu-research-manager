import DeleteIcon from "@mui/icons-material/Delete";
import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  IconButton,
  MenuItem,
  TextField,
} from "@mui/material";
import { useEffect, useState } from "react";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { toast } from "react-toastify";

import {
  createAssignment,
  getAssignmentsOfManager,
  updateAssignment,
} from "../../lib/api/assignmentApi";
import { getDepartments } from "../../lib/api/departmentsApi";

// Assuming the department type has id and name fields.
interface Department {
  id: string;
  name: string;
}

// ManagerRow type passed from AssignmentPage.
export interface ManagerRow {
  managerId: string;
  managerFullName: string;
  managerDepartmentName: string;
  assignedDepartments: {
    departmentId: string;
    assignedDepartmentName: string;
  }[];
  assignedCount: number;
}

interface AssignmentFormProps {
  open: boolean;
  handleClose: () => void;
  manager: ManagerRow;
  onSuccess: () => void;
}

// Form inputs type: an array of assignments (each with a departmentId)
type AssignmentFormInputs = {
  assignments: { departmentId: string }[];
};

export default function AssignmentForm({
  open,
  handleClose,
  manager,
  onSuccess,
}: AssignmentFormProps) {
  const { control, handleSubmit, reset } = useForm<AssignmentFormInputs>({
    defaultValues: {
      assignments: [],
    },
  });

  const { fields, append, remove } = useFieldArray({
    control,
    name: "assignments",
  });

  const [departments, setDepartments] = useState<Department[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  // Flag to decide if we should create (no content, 204) or update assignments.
  const [isNew, setIsNew] = useState<boolean>(false);

  useEffect(() => {
    if (open) {
      setLoading(true);
      async function fetchData() {
        // Fetch all departments
        try {
          const deptRes = await getDepartments();
          setDepartments(deptRes.data || []);
        } catch (error) {
          toast.error("Lỗi khi tải đơn vị: " + (error as Error).message);
        }

        // Fetch current assignments for the manager
        try {
          const res = await getAssignmentsOfManager(manager.managerId);
          // If the response is empty (204 No Content or empty data array), treat as new assignment
          if (
            !res ||
            !res.data ||
            (Array.isArray(res.data) && res.data.length === 0)
          ) {
            reset({ assignments: [] });
            setIsNew(true);
          } else {
            // Prepopulate the form with the current assignments
            const initialAssignments = res.data.map((assignment: any) => ({
              departmentId: assignment.departmentId,
            }));
            reset({ assignments: initialAssignments });
            setIsNew(false);
          }
        } catch (error: any) {
          // In case of an error, assume no assignment exists and treat as new.
          reset({ assignments: [] });
          setIsNew(true);
        }
        setLoading(false);
      }
      fetchData();
    }
  }, [open, manager.managerId, reset]);

  const onSubmit = async (data: AssignmentFormInputs) => {
    const departmentIds = data.assignments
      .map((item) => item.departmentId)
      .filter(Boolean);
    if (departmentIds.length === 0) {
      toast.error("Vui lòng chọn ít nhất 1 đơn vị");
      return;
    }
    try {
      if (isNew) {
        await createAssignment({
          managerId: manager.managerId,
          departmentIds,
        });
        toast.success("Phân công thành công");
      } else {
        await updateAssignment({
          managerId: manager.managerId,
          departmentIds,
        });
        toast.success("Cập nhật phân công thành công");
      }
      onSuccess(); // e.g., queryClient.invalidateQueries({ queryKey: ["assignments"] })
      handleClose();
    } catch (error: any) {
      toast.error("Lỗi khi lưu phân công: " + error.message);
    }
  };

  return (
    <Dialog open={open} onClose={handleClose} fullWidth>
      <DialogTitle>Phân công cho quản lý</DialogTitle>
      <DialogContent>
        {loading ? (
          <Box sx={{ display: "flex", justifyContent: "center", my: 4 }}>
            <CircularProgress />
          </Box>
        ) : (
          <>
            {/* Manager Information */}
            <TextField
              label="Họ và tên"
              value={manager.managerFullName}
              fullWidth
              margin="dense"
              disabled
            />
            <TextField
              label="Đơn vị công tác"
              value={manager.managerDepartmentName}
              fullWidth
              margin="dense"
              disabled
            />

            {/* Dynamic assignment fields */}
            {fields.map((field, index) => (
              <Box
                key={field.id}
                sx={{ display: "flex", alignItems: "center", mt: 2 }}
              >
                <Controller
                  control={control}
                  name={`assignments.${index}.departmentId`}
                  defaultValue={field.departmentId}
                  render={({ field: controllerField }) => (
                    <TextField
                      select
                      label="Chọn đơn vị"
                      value={controllerField.value}
                      onChange={controllerField.onChange}
                      fullWidth
                      margin="dense"
                    >
                      <MenuItem value="">-- Chọn đơn vị --</MenuItem>
                      {departments.map((dept: Department) => (
                        <MenuItem key={dept.id} value={dept.id}>
                          {dept.name}
                        </MenuItem>
                      ))}
                    </TextField>
                  )}
                />

                <IconButton
                  onClick={() => remove(index)}
                  sx={{ ml: 1 }}
                  aria-label="delete"
                >
                  <DeleteIcon />
                </IconButton>
              </Box>
            ))}

            <Button
              onClick={() => append({ departmentId: "" })}
              sx={{ mt: 2 }}
              variant="outlined"
              disabled={loading}
            >
              Thêm đơn vị
            </Button>
          </>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleClose}>Hủy</Button>
        <Button onClick={handleSubmit(onSubmit)} variant="contained">
          Lưu
        </Button>
      </DialogActions>
    </Dialog>
  );
}
