import {
  KeyboardArrowDown as KeyboardArrowDownIcon,
  KeyboardArrowUp as KeyboardArrowUpIcon,
} from "@mui/icons-material";
import CancelIcon from "@mui/icons-material/Cancel";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";
import HistoryIcon from "@mui/icons-material/History";
import {
  Box,
  Button,
  Collapse,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import * as React from "react";
import { ProofStatus } from "../../../../lib/types/enums/ProofStatus";
import { Work } from "../../../../lib/types/models/Work";
import { formatMonthYear } from "../../../../lib/utils/dateUtils";
import { getScoreLevelText } from "../../../../lib/utils/scoreLevelUtils";

interface WorksCollapsibleTableProps {
  works: Work[];
  userId: string;
  onEdit: (work: Work) => void;
  onDelete: (id: string) => void;
  canEditWork: (
    proofStatus: ProofStatus | undefined,
    isLocked: boolean,
  ) => boolean;
  canDeleteWork: (
    proofStatus: ProofStatus | undefined,
    isLocked: boolean,
    hasOtherValid: boolean,
  ) => boolean;
}

function Row({
  work,
  userId,
  onEdit,
  onDelete,
  canEditWork,
  canDeleteWork,
}: {
  work: WorksCollapsibleTableProps["works"][0];
} & Pick<
  WorksCollapsibleTableProps,
  "userId" | "onEdit" | "onDelete" | "canEditWork" | "canDeleteWork"
>) {
  const [open, setOpen] = React.useState(false);
  const currentAuthor = work.authors?.find((a) => a.userId === userId);
  const proofStatus = currentAuthor?.proofStatus;
  const isLocked = work.isLocked;
  const hasOtherValid =
    work.authors?.some(
      (a) => a.userId !== userId && a.proofStatus === ProofStatus.HopLe,
    ) ?? false;

  // render proofStatus icon+text
  const renderStatus = () => {
    switch (proofStatus) {
      case ProofStatus.HopLe:
        return (
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <CheckCircleIcon color="success" /> Hợp lệ
          </Box>
        );
      case ProofStatus.KhongHopLe:
        return (
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <CancelIcon color="error" /> Không hợp lệ
          </Box>
        );
      case ProofStatus.ChuaXuLy:
        return (
          <Box sx={{ display: "flex", alignItems: "center", gap: 1 }}>
            <HistoryIcon color="action" /> Chưa xử lý
          </Box>
        );
      default:
        return "-";
    }
  };

  return (
    <React.Fragment>
      {/* Main Row */}
      <TableRow sx={{ "& > *": { borderBottom: "unset" } }}>
        <TableCell padding="checkbox">
          <IconButton size="small" onClick={() => setOpen(!open)}>
            {open ? <KeyboardArrowUpIcon /> : <KeyboardArrowDownIcon />}
          </IconButton>
        </TableCell>
        <TableCell>{work.title}</TableCell>
        <TableCell>{work.workTypeName}</TableCell>
        <TableCell>{work.workLevelName}</TableCell>
        <TableCell>{currentAuthor?.purposeName ?? "-"}</TableCell>
        <TableCell>
          {/* same logic as your DataGrid actions cell */}
          {isLocked && proofStatus === ProofStatus.HopLe ? (
            <Box sx={{ display: "flex", gap: 1 }}>
              <Button variant="contained" color="primary" size="small" disabled>
                {" "}
                Sửa{" "}
              </Button>
              <Button variant="contained" color="error" size="small" disabled>
                {" "}
                Xóa{" "}
              </Button>
            </Box>
          ) : (
            <Box sx={{ display: "flex", gap: 1 }}>
              <Button
                size="small"
                variant="contained"
                color="primary"
                disabled={!canEditWork(proofStatus, isLocked)}
                onClick={() => onEdit(work)}
              >
                {" "}
                Sửa{" "}
              </Button>
              <Button
                size="small"
                variant="contained"
                color="error"
                disabled={!canDeleteWork(proofStatus, isLocked, hasOtherValid)}
                onClick={() => onDelete(work.id)}
              >
                {" "}
                Xóa{" "}
              </Button>
            </Box>
          )}
        </TableCell>
      </TableRow>

      {/* Collapsible Row */}
      <TableRow>
        <TableCell style={{ padding: 0 }} colSpan={6}>
          <Collapse in={open} timeout="auto" unmountOnExit>
            <Box sx={{ margin: 1 }}>
              <Table size="small">
                <TableBody>
                  {/* just map every other field here */}
                  <TableRow>
                    <TableCell>Thời gian xuất bản</TableCell>
                    <TableCell>
                      {work.timePublished
                        ? formatMonthYear(work.timePublished)
                        : "-"}
                    </TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Số tác giả</TableCell>
                    <TableCell>{work.totalAuthors}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Đồng tác giả</TableCell>
                    <TableCell>{work.coAuthorUserIds.length}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Vai trò tác giả</TableCell>
                    <TableCell>{currentAuthor?.authorRoleName}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Vị trí</TableCell>
                    <TableCell>{currentAuthor?.position}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Thông tin chi tiết</TableCell>
                    <TableCell>
                      {work.details
                        ? Object.entries(work.details).map(([key, value]) => (
                            <div key={key} style={{ marginBottom: 4 }}>
                              <strong>{key}</strong>: {value}
                            </div>
                          ))
                        : "-"}
                    </TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Ngành tính điểm</TableCell>
                    <TableCell>{currentAuthor?.fieldName ?? "-"}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Mức điểm</TableCell>
                    <TableCell>
                      {currentAuthor?.scoreLevel != null
                        ? getScoreLevelText(currentAuthor.scoreLevel)
                        : "-"}
                    </TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Giờ tác giả</TableCell>
                    <TableCell>{currentAuthor?.authorHour}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Trạng thái</TableCell>
                    <TableCell>{renderStatus()}</TableCell>
                  </TableRow>
                  <TableRow>
                    <TableCell>Ghi chú</TableCell>
                    <TableCell>{currentAuthor?.note ?? "-"}</TableCell>
                  </TableRow>
                  {/* add more rows for coAuthors, hours, etc. */}
                </TableBody>
              </Table>
            </Box>
          </Collapse>
        </TableCell>
      </TableRow>
    </React.Fragment>
  );
}

export default function WorksCollapsibleTable(
  props: WorksCollapsibleTableProps,
) {
  const { works, userId, onEdit, onDelete, canEditWork, canDeleteWork } = props;

  return (
    <TableContainer component={Paper}>
      <Table aria-label="collapsible works table">
        <TableHead>
          <TableRow>
            <TableCell />
            <TableCell>Tên công trình</TableCell>
            <TableCell>Loại công trình</TableCell>
            <TableCell>Cấp công trình</TableCell>
            <TableCell>Mục đích quy đổi</TableCell>
            <TableCell>Thao tác</TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {works.map((w) => (
            <Row
              key={w.id}
              work={w}
              userId={userId}
              onEdit={onEdit}
              onDelete={onDelete}
              canEditWork={canEditWork}
              canDeleteWork={canDeleteWork}
            />
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
}
