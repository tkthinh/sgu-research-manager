import { Box } from "@mui/material";
import { Account } from "@toolpad/core";
import AccountInfo from "./AccountInfo";
import NotificationDropdown from "./NotificationDropdown";

export default function CustomAccount() {
  return (
    <Box display={"flex"} alignItems="center" gap={2}>
      <NotificationDropdown />
      <Account
        slots={{
          popoverContent: AccountInfo,
        }}
        localeText={{
          signOutLabel: "Đăng xuất",
        }}
      />
    </Box>
  );
}
