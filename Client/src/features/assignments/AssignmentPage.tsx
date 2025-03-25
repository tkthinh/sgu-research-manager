import {
  Box,
  Button,
  CircularProgress,
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  Paper,
  Typography,
} from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import GenericTable from "../../app/shared/components/tables/DataTable";
import {
  deleteAssignmentsOfUser,
  getAssignments,
} from "../../lib/api/assignmentApi";
import AssignmentForm from "./AssignmentForm";

// Define the type for a grouped manager row
interface ManagerRow {
  managerId: string;
  managerFullName: string;
  managerDepartmentName: string;
  assignedDepartments: {
    departmentId: string;
    assignedDepartmentName: string;
  }[];
  assignedCount: number;
}

export default function AssignmentPage() {
  const queryClient = useQueryClient();

  // Fetch assignments
  const { data, error, isPending, isSuccess, dataUpdatedAt, refetch } =
    useQuery({
      queryKey: ["assignments"],
      queryFn: getAssignments,
    });

  // Toast notifications similar to UserPage
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message || "Lấy dữ liệu phân công thành công", {
        toastId: "fetch-assignments-success",
      });
    }
  }, [dataUpdatedAt, isSuccess, data]);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi xảy ra: " + (error as Error).message);
    }
  }, [error]);

  // Process API response and group assignments by manager
  const groupedData: ManagerRow[] = data?.data
    ? groupAssignments(data.data)
    : [];

  // State to control the assignment form dialog
  const [openForm, setOpenForm] = useState(false);
  const [selectedManager, setSelectedManager] = useState<ManagerRow | null>(
    null,
  );

  // State for deletion confirmation dialog
  const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
  const [managerIdToUnassign, setManagerIdToUnassign] = useState<string | null>(
    null,
  );

  const handleOpenForm = (manager: ManagerRow) => {
    console.log(manager);
    setSelectedManager(manager);
    setOpenForm(true);
  };
  const handleCloseForm = () => {
    setOpenForm(false);
    setSelectedManager(null);
  };

  // Mutation for unassigning (clearing assignments) for a manager
  const unassignMutation = useMutation({
    mutationFn: (managerId: string) => deleteAssignmentsOfUser(managerId),
    onSuccess: () => {
      toast.success("Đã bỏ phân công thành công");
      queryClient.invalidateQueries({ queryKey: ["assignments"] });
      refetch();
    },
    onError: (error: any) => {
      toast.error("Có lỗi xảy ra khi bỏ phân công: " + error.message);
    },
  });

  // Open the deletion confirmation dialog instead of window.confirm
  const handleUnassign = (managerId: string) => {
    setManagerIdToUnassign(managerId);
    setDeleteDialogOpen(true);
  };

  const confirmUnassign = () => {
    if (managerIdToUnassign) {
      unassignMutation.mutate(managerIdToUnassign);
      setDeleteDialogOpen(false);
    }
  };

  const columns: GridColDef[] = [
    { field: "managerFullName", headerName: "Họ và tên quản lý", width: 200 },
    {
      field: "managerDepartmentName",
      headerName: "Đơn vị công tác",
      width: 200,
    },
    { field: "assignedCount", headerName: "Số phân công", width: 150 },
    {
      field: "actions",
      headerName: "",
      width: 300,
      renderCell: (params) => (
        <Box>
          <Button
            variant="contained"
            size="small"
            onClick={() => handleOpenForm(params.row)}
          >
            Phân công
          </Button>
          <Button
            variant="contained"
            color="error"
            size="small"
            onClick={() => handleUnassign(params.row.managerId)}
            disabled={params.row.assignedCount === 0}
            sx={{ ml: 1 }}
          >
            Bỏ phân công
          </Button>
        </Box>
      ),
    },
  ];

  if (isPending) return <CircularProgress />;
  if (error)
    return (
      <Typography variant="body1">Error: {(error as Error).message}</Typography>
    );

  const rowsWithId = groupedData.map((row) => ({ id: row.managerId, ...row }));

  return (
    <>
      <Paper sx={{ width: "100%", marginX: "auto", padding: 2 }}>
        <GenericTable columns={columns} data={rowsWithId} />
      </Paper>

      {selectedManager && (
        <AssignmentForm
          open={openForm}
          handleClose={handleCloseForm}
          manager={selectedManager}
          onSuccess={() => {
            queryClient.invalidateQueries({ queryKey: ["assignments"] });
            refetch();
          }}
        />
      )}

      <Dialog
        open={deleteDialogOpen}
        onClose={() => setDeleteDialogOpen(false)}
      >
        <DialogTitle>Xác nhận bỏ phân công</DialogTitle>
        <DialogContent>
          <Typography>
            Bạn có chắc chắn muốn bỏ <strong>tất cả</strong> phân công cho quản
            lý này?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteDialogOpen(false)}>Hủy</Button>
          <Button onClick={confirmUnassign} color="error" variant="contained">
            {unassignMutation.isPending ? "Đang xóa..." : "Xóa"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}

// Helper function: groups assignments by manager
function groupAssignments(assignments: any[]): ManagerRow[] {
  const groups = new Map<string, ManagerRow>();
  assignments.forEach((assignment) => {
    const {
      managerId,
      managerFullName,
      managerDepartmentName,
      departmentId,
      assignedDepartmentName,
    } = assignment;

    if (!groups.has(managerId)) {
      groups.set(managerId, {
        managerId,
        managerFullName,
        managerDepartmentName,
        assignedDepartments: [],
        assignedCount: 0,
      });
    }
    if (
      departmentId !== "00000000-0000-0000-0000-000000000000" &&
      assignedDepartmentName !== "Chưa phân công"
    ) {
      groups.get(managerId)!.assignedDepartments.push({
        departmentId,
        assignedDepartmentName,
      });
    }
  });
  // Compute the assigned count and return an array
  return Array.from(groups.values()).map((group) => ({
    ...group,
    assignedCount: group.assignedDepartments.length,
  }));
}
