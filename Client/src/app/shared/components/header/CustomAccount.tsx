import { Account } from "@toolpad/core";
import AccountInfo from "./AccountInfo";

export default function CustomAccount() {
  return (
    <Account slots={{
      popoverContent: AccountInfo
    }}
    localeText={{
      signOutLabel: "Đăng xuất"
    }}
    />
  )
}