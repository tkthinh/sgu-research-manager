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
import { ProofStatus } from "../../../../lib/types/enums/ProofStatus";
import { useEffect } from "react";
import { useAuth } from "../../../../app/shared/contexts/AuthContext";

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
  const { user } = useAuth();
  const currentAuthor = selectedWork?.authors?.find(author => author.userId === user?.id);
  const proofStatus = currentAuthor?.proofStatus;
  const isLocked = selectedWork?.isLocked;

  // Nếu công trình bị khóa và tác giả hiện tại chưa được chấm hợp lệ
  // hoặc đang ở tab thông tin công trình thì chuyển sang tab thông tin tác giả
  useEffect(() => {
    if (isLocked && proofStatus !== 0) {
      setActiveTab(1);
    }
  }, [isLocked, proofStatus, setActiveTab]);

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
      <DialogContent>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', mb: 2 }}>
          <Tabs 
            value={activeTab} 
            onChange={(_, newValue) => {
              // Nếu công trình bị khóa và tác giả chưa được chấm hợp lệ
              // thì không cho chuyển sang tab thông tin công trình
              if (isLocked && proofStatus !== 0 && newValue === 0) {
                return;
              }
              setActiveTab(newValue);
            }}
          >
            <Tab 
              label="Thông tin công trình" 
              disabled={isLocked && proofStatus !== 0}
              sx={{ 
                color: isLocked && proofStatus !== 0 ? 'text.disabled' : 'inherit',
                '&.Mui-disabled': {
                  color: 'text.disabled'
                }
              }}
            />
            <Tab label="Thông tin tác giả" />
          </Tabs>
        </Box>
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