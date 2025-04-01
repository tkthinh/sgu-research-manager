import { Divider, Grid, Link, Stack, Typography } from "@mui/material";
import { Session, useSession } from "@toolpad/core";
import { AccountPopoverFooter, SignOutButton } from "@toolpad/core/Account";
import { User } from "../../../../lib/types/models/User";
import { getAcademicTitle } from "../../../../lib/utils/academicTitleMap";
import { getOfficerRank } from "../../../../lib/utils/officerRankMap";

interface CustomSession extends Session {
  user: User;
}

export default function AccountInfo() {
  const session = useSession<CustomSession>();
  if (!session || !session.user) return null;

  return (
    <Stack
      sx={{
        maxWidth: 350,
        padding: 2,
      }}
    >
      {/* <AccountPreview variant="expanded" /> */}
      {session.user && (
        <Stack mb={1}>
          <Typography variant="h6" fontWeight={600}>
            {session.user.fullName}
          </Typography>
          <Typography variant="body2">
            {session.user.fieldName}
          </Typography>
          <Typography variant="body2" fontWeight={300}>
            {session.user.userName}
          </Typography>
          <Divider sx={{ my: 1 }} />
          <Grid container spacing={2}>
            <Grid item xs={6}>
              <Typography variant="body2">Email:</Typography>
            </Grid>
            <Grid item xs={6}>
              <Typography variant="body2">{session.user.email}</Typography>
            </Grid>

            <Grid item xs={6}>
              <Typography variant="body2">Học vị:</Typography>
            </Grid>
            <Grid item xs={6}>
              <Typography variant="body2">
                {getAcademicTitle(session.user.academicTitle)}
              </Typography>
            </Grid>

            <Grid item xs={6}>
              <Typography variant="body2">Ngạch viên chức:</Typography>
            </Grid>
            <Grid item xs={6}>
              <Typography variant="body2">
                {getOfficerRank(session.user.officerRank)}
              </Typography>
            </Grid>

            <Grid item xs={6}>
              <Typography variant="body2">Đơn vị công tác:</Typography>
            </Grid>
            <Grid item xs={6}>
              <Typography variant="body2">
                {session.user.departmentName}
              </Typography>
            </Grid>
          </Grid>

          {/* <Typography variant="body2">Role: {session.user.role}</Typography> */}
          <Link href="/cap-nhat-thong-tin" sx={{ marginTop: 2 }}>
            Chỉnh sửa thông tin
          </Link>
        </Stack>
      )}
      <Divider />
      <AccountPopoverFooter>
        <SignOutButton />
      </AccountPopoverFooter>
    </Stack>
  );
}
