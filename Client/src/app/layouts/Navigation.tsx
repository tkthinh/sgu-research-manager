import { Navigation } from "@toolpad/core";
import DashboardIcon from '@mui/icons-material/Dashboard';
import ConstructionIcon from '@mui/icons-material/Construction';
import RateReviewIcon from '@mui/icons-material/RateReview';
import AssignmentIndIcon from '@mui/icons-material/AssignmentInd';
import LeaderboardIcon from '@mui/icons-material/Leaderboard';
import ManageAccountsIcon from '@mui/icons-material/ManageAccounts';
import ManageHistoryIcon from '@mui/icons-material/ManageHistory';
import SettingsIcon from '@mui/icons-material/Settings';
import RestorePageIcon from '@mui/icons-material/RestorePage';
import LibraryAddIcon from '@mui/icons-material/LibraryAdd';

export const NAVIGATION: Navigation = [
  {
    kind: 'header',
    title: 'Chính',
  },
  {
    title: 'Trang chủ',
    icon: <DashboardIcon />,
  },
  {
    segment: 'cong-trinh',
    title: 'Công trình',
    icon: <ConstructionIcon />,
  },
  {
    segment: 'quy-doi',
    title: 'Đăng ký quy đổi',
    icon: <LibraryAddIcon />,
  },
  {
    segment: 'cham-diem',
    title: 'Chấm điểm công trình',
    icon: <RateReviewIcon />,
  },
  {
    segment: 'phan-cong',
    title: 'Phân công chấm điểm',
    icon: <AssignmentIndIcon />,
  },
  {
    kind: 'header',
    title: 'Báo cáo',
  },
  {
    segment: 'bao-cao',
    title: 'Báo cáo',
    icon: <LeaderboardIcon />,
  },
  {
    kind: 'header',
    title: 'Hệ thống',
  },
  {
    segment: 'quan-ly-tai-khoan',
    title: 'Quản lý tài khoản',
    icon: <ManageAccountsIcon />,
  },
  {
    segment: 'quan-ly-thoi-gian',
    title: 'Quản lý thời gian',
    icon: <ManageHistoryIcon />,
  },
  // {
  //   segment: 'lich-su-upload',
  //   title: 'Lịch sử upload',
  //   icon: <RestorePageIcon />,
  // },
  {
    segment: 'he-thong',
    title: 'Cấu hình hệ thống',
    icon: <SettingsIcon />,
    children: [
      {
        segment: 'he-so-cham-diem',
        title: 'Hệ số chấm điểm',
      },
      {
        segment: 'vai-tro-tac-gia',
        title: 'Vai trò tác giả',
      },
      {
        segment: 'muc-dich-quy-doi',
        title: 'Mục đích quy đổi',
      },
      {
        kind: 'divider',
      },
      {
        segment: 'loai-cong-trinh',
        title: 'Loại công trình',
      },
      {
        segment: 'cap-cong-trinh',
        title: 'Cấp công trình',
      },
      {
        segment: 'nganh-scimago',
        title: 'Ngành SCImago',
      },
      {
        kind: 'divider',
      },
      {
        segment: 'nganh',
        title: 'Ngành',
      },
      {
        segment: 'don-vi',
        title: 'Đơn vị',
      },
    ]
  },
];