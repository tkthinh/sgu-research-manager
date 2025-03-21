import { Navigation } from "@toolpad/core";
import DashboardIcon from '@mui/icons-material/Dashboard';
import ConstructionIcon from '@mui/icons-material/Construction';
import AssignmentIndIcon from '@mui/icons-material/AssignmentInd';
import LeaderboardIcon from '@mui/icons-material/Leaderboard';
import ManageAccountsIcon from '@mui/icons-material/ManageAccounts';
import ManageHistoryIcon from '@mui/icons-material/ManageHistory';
import SettingsIcon from '@mui/icons-material/Settings';
import RateReviewIcon from '@mui/icons-material/RateReview';
import RestorePageIcon from '@mui/icons-material/RestorePage';
import LibraryAddCheckIcon from '@mui/icons-material/LibraryAddCheck';

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
    segment: 'cham-diem',
    title: 'Chấm điểm công trình',
    icon: <RateReviewIcon />,
  },
  {
    segment: 'dang-ky-quy-doi',
    title: 'Đăng ký quy đổi',
    icon: <LibraryAddCheckIcon />,
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
    segment: 'cau-hinh-he-thong',
    title: 'Cấu hình hệ thống',
    icon: <ManageHistoryIcon />,
  },
  // {
  //   segment: 'lich-su-upload',
  //   title: 'Lịch sử upload',
  //   icon: <RestorePageIcon />,
  // },
  {
    segment: 'cai-dat',
    title: 'Quản lý danh mục',
    icon: <SettingsIcon />,
    children: [
      {
        segment: 'loai-cong-trinh',
        title: 'Loại công trình',
      },
      {
        segment: 'cap-cong-trinh',
        title: 'Cấp công trình',
      },
      {
        segment: 'he-so-quy-doi',
        title: 'Hệ số quy đổi',
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
        segment: 'tinh-trang-minh-chung',
        title: 'Tình trạng minh chứng',
      },
      {
        kind: 'divider',
      },
      {
        segment: 'nganh-scimago',
        title: 'Ngành SCImago',
      },
      {
        segment: 'nganh',
        title: 'Ngành',
      },
      {
        segment: 'tinh-trang-cong-trinh',
        title: 'Tình trạng công trình',
      },
      {
        kind: 'divider',
      },
      {
        segment: 'don-vi',
        title: 'Đơn vị',
      },
    ]
  },
];