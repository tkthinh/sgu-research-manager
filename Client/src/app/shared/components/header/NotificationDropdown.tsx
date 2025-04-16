import { HubConnectionBuilder } from "@microsoft/signalr";
import NotificationsIcon from "@mui/icons-material/Notifications";
import {
  Badge,
  CircularProgress,
  IconButton,
  Menu,
  MenuItem,
  Typography,
} from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import { getMyNotifications } from "../../../../lib/api/notificationsApi";

export default function NotificationDropdown() {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const queryClient = useQueryClient();

  // Load notification list
  const { data, isLoading } = useQuery({
    queryKey: ["notifications"],
    queryFn: getMyNotifications,
  });

  // // Handle real-time notification
  // useEffect(() => {
  //   const connection = new HubConnectionBuilder()
  //     .withUrl(import.meta.env.VITE_HUB_URL + "?access_token=" + localStorage.getItem("token"))
  //     .withAutomaticReconnect()
  //     .build();

  //   connection.on("ReceiveNotification", (notification) => {
  //     queryClient.setQueryData(["notifications"], (old: any) => {
  //       if (!old?.data) return old;
  //       return {
  //         ...old,
  //         data: [notification, ...old.data],
  //       };
  //     });
  //   });

  //   connection.start().catch(console.error);
  //   return () => {
  //     connection.stop();
  //   };
  // }, [queryClient]);

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const notifications = data?.data || [];
  const unreadCount = notifications.filter((n) => !n.isRead).length;

  console.log("Notifications: ", notifications);

  return (
    <div>
      <IconButton
        id="notification-button"
        aria-controls={open ? "notification-menu" : undefined}
        aria-haspopup="true"
        aria-expanded={open ? "true" : undefined}
        onClick={handleClick}
      >
        <Badge badgeContent={unreadCount} color="error">
          <NotificationsIcon color="primary" />
        </Badge>
      </IconButton>

      <Menu
        id="notification-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          "aria-labelledby": "notification-button",
        }}
      >
        {isLoading ? (
          <MenuItem sx={{ p: 1 }}>
            <CircularProgress size={20} />
          </MenuItem>
        ) : notifications.length > 0 ? (
          notifications.map((n) => (
            <MenuItem key={n.id} onClick={handleClose}>
              <p>{n.content}</p>
              <p>{n.createdDate.toString()}</p>
            </MenuItem>
          ))
        ) : (
          <Typography sx={{ p: 1 }} variant="body1">
            Không có thông báo nào để hiển thị.
          </Typography>
        )}
      </Menu>
    </div>
  );
}
