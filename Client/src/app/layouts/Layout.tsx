import { DashboardLayout } from "@toolpad/core/DashboardLayout";
import { PageContainer } from "@toolpad/core/PageContainer";
import { Outlet } from "react-router";
import CustomAccount from "../shared/components/header/CustomAccount";

export default function Layout() {
  return (
    <DashboardLayout
      slots={{
        toolbarAccount: CustomAccount,
      }}
    >
      <PageContainer>
        <Outlet />
      </PageContainer>
    </DashboardLayout>
  );
}
