import { Account } from "@toolpad/core";
import AccountInfo from "./AccountInfo";
import { Badge, Box } from "@mui/material";
import NotificationsIcon from '@mui/icons-material/Notifications';

export default function CustomAccount() {
  return (
    <Box display={'flex'} alignItems="center" gap={2}>
    <Badge badgeContent={4} color="error">
      <NotificationsIcon height={16} color="primary"/>
    </Badge>
    <Account slots={{
      popoverContent: AccountInfo
    }}
    localeText={{
      signOutLabel: "Đăng xuất"
    }}
    />
    </Box>
  )
}