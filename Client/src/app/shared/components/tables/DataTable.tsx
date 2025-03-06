import { DataGrid, GridColDef } from "@mui/x-data-grid";
import { viVN } from '@mui/x-data-grid/locales';
import React from "react";

interface GenericTableProps {
  columns: GridColDef[];
  data: any[];
}

const GenericTable: React.FC<GenericTableProps> = ({ columns, data }) => {
  const paginationModel = { page: 0, pageSize: 10 };

  return (
    <DataGrid
      rows={data}
      columns={columns}
      initialState={{ pagination: { paginationModel } }}
      pageSizeOptions={[10, 15, 25]}
      disableRowSelectionOnClick={true}
      localeText={viVN.components.MuiDataGrid.defaultProps.localeText}
    />
  );
};

export default GenericTable;
