import { Alert, CircularProgress, Typography } from "@mui/material";
import { useQuery } from "@tanstack/react-query";
import { useEffect } from "react";
import { toast } from "react-toastify";
import { getGlobalNotifications } from "../../lib/api/notificationsApi";

export default function Dashboard() {
  const { data, isLoading, isSuccess, error } = useQuery({
    queryKey: ["global-notifications"],
    queryFn: getGlobalNotifications,
  });

  // Toast notifications
  useEffect(() => {
    if (isSuccess && data) {
      toast.success(data.message, {
        toastId: "fetch-global-notifications-success",
      });
    }
  }, []);

  useEffect(() => {
    if (error) {
      toast.error("Có lỗi đã xảy ra: " + (error as Error).message);
    }
  }, [error]);

  if (isLoading) return <CircularProgress />;
  if (!data?.data || !Array.isArray(data.data))
    return <div>Không có thông báo nào để hiển thị.</div>;

  return (
    <>
      {data.data.map((notification) => (
        <Alert key={notification.id} severity="info" sx={{ mb: 2 }}>
          <Typography variant="body1">{notification.content}</Typography>
        </Alert>
      ))}
    </>
  );
}
