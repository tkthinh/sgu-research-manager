import React from "react";
import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Button,
  Box,
  Tabs,
  Tab,
  CircularProgress,
} from "@mui/material";
import { Work } from "../../../../lib/types/models/Work";
import WorkForm from "../../../../features/works/WorkForm";

interface WorkUpdateDialogProps {
  open: boolean;
  onClose: () => void;
  selectedWork: Work | null;
  activeTab: number;
  setActiveTab: (tab: number) => void;
  onSubmit: (data: any) => void;
  isPending: boolean;
  workTypes: any[];
  workLevels: any[];
  authorRoles: any[];
  purposes: any[];
  scimagoFields: any[];
  fields: any[];
}

/**
 * Dialog cập nhật công trình tái sử dụng
 */
export default function WorkUpdateDialog({
  open,
  onClose,
  selectedWork,
  activeTab,
  setActiveTab,
  onSubmit,
  isPending,
  workTypes,
  workLevels,
  authorRoles,
  purposes,
  scimagoFields,
  fields,
}: WorkUpdateDialogProps) {
  return (
    <Dialog
      open={open}
      onClose={onClose}
      fullWidth
      maxWidth="md"
    >
      <DialogTitle>
        {selectedWork ? "Cập nhật công trình" : "Thêm công trình mới"}
      </DialogTitle>
      <Box sx={{ borderBottom: 1, borderColor: 'divider', px: 3 }}>
        <Tabs value={activeTab} onChange={(_, newValue) => setActiveTab(newValue)}>
          <Tab label="Thông tin công trình" />
          <Tab label="Thông tin tác giả" />
        </Tabs>
      </Box>
      <DialogContent>
        {workTypes.length > 0 ? (
          <WorkForm
            initialData={selectedWork}
            onSubmit={onSubmit}
            isLoading={isPending}
            workTypes={workTypes}
            workLevels={workLevels}
            authorRoles={authorRoles}
            purposes={purposes}
            scimagoFields={scimagoFields}
            fields={fields}
            activeTab={activeTab}
          />
        ) : (
          <Box sx={{ display: "flex", justifyContent: "center", p: 3 }}>
            <CircularProgress />
          </Box>
        )}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="inherit">
          Đóng
        </Button>
      </DialogActions>
    </Dialog>
  );
} 