import { Navigation } from "@toolpad/core";
import DashboardIcon from '@mui/icons-material/Dashboard';
import ConstructionIcon from '@mui/icons-material/Construction';
import AssignmentIndIcon from '@mui/icons-material/AssignmentInd';
import LeaderboardIcon from '@mui/icons-material/Leaderboard';
import ManageAccountsIcon from '@mui/icons-material/ManageAccounts';
import ManageHistoryIcon from '@mui/icons-material/ManageHistory';
import SettingsIcon from '@mui/icons-material/Settings';
import RateReviewIcon from '@mui/icons-material/RateReview';
import LibraryAddCheckIcon from '@mui/icons-material/LibraryAddCheck';
import AssessmentIcon from '@mui/icons-material/Assessment';

// Navigation for "User" role
export const NAVIGATION_USER: Navigation = [
  {
    kind: "header",
    title: "Chính",
  },
  {
    title: "Trang chủ",
    icon: <DashboardIcon />,
  },
  {
    segment: "cong-trinh",
    title: "Công trình",
    icon: <ConstructionIcon />,
  },
  {
    segment: "dang-ky-quy-doi",
    title: "Đăng ký quy đổi",
    icon: <LibraryAddCheckIcon />,
  },
  {
    kind: "header",
    title: "Báo cáo",
  },
  {
    segment: "bao-cao",
    title: "Báo cáo",
    icon: <LeaderboardIcon />,
  }
];

// Navigation for "Manager" role
export const NAVIGATION_MANAGER: Navigation = [
  {
    kind: "header",
    title: "Chính",
  },
  {
    title: "Trang chủ",
    icon: <DashboardIcon />,
  },
  {
    segment: "cong-trinh",
    title: "Công trình",
    icon: <ConstructionIcon />,
  },
  {
    segment: "cham-diem",
    title: "Chấm điểm công trình",
    icon: <RateReviewIcon />,
  },
  {
    kind: "header",
    title: "Báo cáo",
  },
  {
    segment: "bao-cao",
    title: "Báo cáo",
    icon: <LeaderboardIcon />,
  },
  {
    segment: "thong-ke",
    title: "Thống kê",
    icon: <AssessmentIcon />,
  },
  {
    kind: "header",
    title: "Hệ thống",
  },
  {
    segment: "quan-ly-tai-khoan",
    title: "Quản lý tài khoản",
    icon: <ManageAccountsIcon />,
  },
];

// Navigation for "Admin" role
export const NAVIGATION_ADMIN: Navigation = [
  {
    kind: "header",
    title: "Chính",
  },
  {
    title: "Trang chủ",
    icon: <DashboardIcon />,
  },
  {
    segment: "cong-trinh",
    title: "Công trình",
    icon: <ConstructionIcon />,
  },
  {
    segment: "cham-diem",
    title: "Chấm điểm công trình",
    icon: <RateReviewIcon />,
  },
  {
    segment: "phan-cong",
    title: "Phân công chấm điểm",
    icon: <AssignmentIndIcon />,
  },
  {
    kind: "header",
    title: "Báo cáo",
  },
  {
    segment: "thong-ke",
    title: "Thống kê",
    icon: <AssessmentIcon />,
  },
  {
    kind: "header",
    title: "Hệ thống",
  },
  {
    segment: "quan-ly-tai-khoan",
    title: "Quản lý tài khoản",
    icon: <ManageAccountsIcon />,
  },
  {
    segment: "cau-hinh-he-thong",
    title: "Cấu hình hệ thống",
    icon: <ManageHistoryIcon />,
  },
  {
    segment: "cai-dat",
    title: "Quản lý danh mục",
    icon: <SettingsIcon />,
    children: [
      {
        segment: "he-so-quy-doi",
        title: "Hệ số quy đổi",
      },
      {
        segment: "muc-dich-quy-doi",
        title: "Mục đích quy đổi",
      },
      {
        segment: "vai-tro-tac-gia",
        title: "Vai trò tác giả",
      },
      {
        kind: "divider",
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
        segment: "nganh-scimago",
        title: "Ngành SCImago",
      },
      {
        kind: "divider",
      },
      {
        segment: "don-vi",
        title: "Đơn vị",
      },
      {
        segment: "nganh",
        title: "Ngành",
      },
      {
        segment: "nam-hoc",
        title: "Năm học",
      },
    ],
  },
];
