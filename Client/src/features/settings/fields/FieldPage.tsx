import { Box, Button, CircularProgress, Paper } from "@mui/material";
import { GridColDef } from "@mui/x-data-grid";
import { useQuery } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import GenericTable from "../../../app/shared/components/tables/DataTable";
import { getFields } from "../../../lib/api/fieldsApi";
import FieldForm from "./FieldForm";
import { toast } from "react-toastify";

export default function FieldPage() {
  // Fetch data
  const { data, error, isPending, isSuccess } = useQuery({
    queryKey: ["fields"],
    queryFn: getFields,
  });

  // Toast message
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message);
    }
  }, [isSuccess, data]);

  useEffect(() => {
    if (error) {
      toast.error("Error fetching departments: " + (error as Error).message);
    }
  }, [error]);

  // Handle form dialog
  const [open, setOpen] = useState(false);
  const [selectedData, setSelectedData] = useState(null);

  const handleOpen = (data) => {
    setSelectedData(data);
    setOpen(true);
  };

  const handleClose = () => setOpen(false);

  const handleEdit = (row) => {
    handleOpen(row);
  };

  // Handle deletion
  const handleDelete = (id) => {
    // Implement delete logic here
    console.log("Delete ID:", id);
  };

  const columns: GridColDef[] = [
    // { field: "id", headerName: "Id", type: "string", width: 200 },
    { field: "name", headerName: "Tên ngành", type: "string", width: 500 },
    {
      field: "actions",
      headerName: "",
      width: 500,
      renderCell: (params) => (
        <Box>
          <Button
            variant="contained"
            color="primary"
            size="small"
            onClick={() => handleEdit(params.row)}
            sx={{ marginRight: 1 }}
          >
            Sửa
          </Button>
          <Button
            variant="contained"
            color="error"
            size="small"
            onClick={() => handleDelete(params.row.id)}
          >
            Xóa
          </Button>
        </Box>
      ),
    },
  ];

  if (isPending) return <CircularProgress />;
  if (error) return <p>Error: {(error as Error).message}</p>;

  return (
    <>
      <Box
        display={"flex"}
        flexDirection={"column-reverse"}
        alignItems={"flex-end"}
        sx={{ marginBottom: 2 }}
      >
        <Button variant="contained" onClick={() => handleOpen(null)}>
          Thêm ngành
        </Button>
      </Box>
      <Paper sx={{ width: 1010, marginX: 'auto' }}>
        <GenericTable columns={columns} data={data?.data || []} />
      </Paper>
      <FieldForm open={open} handleClose={handleClose} data={selectedData} />
    </>
  );
}
