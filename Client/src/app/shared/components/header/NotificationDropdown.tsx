import { HubConnectionBuilder } from "@microsoft/signalr";
import NotificationsIcon from "@mui/icons-material/Notifications";
import {
  Badge,
  Box,
  Button,
  CircularProgress,
  IconButton,
  Menu,
  MenuItem,
  Typography,
} from "@mui/material";
import { useQuery, useQueryClient } from "@tanstack/react-query";
import { useEffect, useState } from "react";
import {
  getMyNotifications,
  markNotificationAsRead,
} from "../../../../lib/api/notificationsApi";
import { formatDateTime } from "../../../../lib/utils/dateTimeFormatter";

export default function NotificationDropdown() {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const queryClient = useQueryClient();

  // Fetch notifications
  const { data, isLoading, refetch } = useQuery({
    queryKey: ["notifications"],
    queryFn: getMyNotifications,
  });

  // Real-time updates
  useEffect(() => {
    const connection = new HubConnectionBuilder()
      .withUrl(
        `${import.meta.env.VITE_HUB_URL}?access_token=${localStorage.getItem("token")}`,
      )
      .withAutomaticReconnect()
      .build();

    connection.on("ReceiveNotification", () => {
      refetch();
    });

    connection.start().catch(console.error);
    return () => {
      connection.stop();
    };
  }, [refetch]);

  const notifications = data?.data || [];
  const unreadCount = notifications.filter((n) => !n.isRead).length;

  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleClose = () => {
    setAnchorEl(null);
  };

  const handleNotificationClick = async (id: string) => {
    try {
      await markNotificationAsRead(id);
      queryClient.invalidateQueries({ queryKey: ["notifications"] });
      refetch();
    } catch (error) {
      console.error("Error marking notification as read", error);
    } finally {
      handleClose();
    }
  };

  return (
    <>
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
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        transformOrigin={{ vertical: "top", horizontal: "right" }}
        PaperProps={{ sx: { minWidth: 300, maxHeight: 400 } }}
        MenuListProps={{ "aria-labelledby": "notification-button" }}
      >
        {isLoading ? (
          <MenuItem sx={{ p: 1 }}>
        <CircularProgress size={20} />
          </MenuItem>
        ) : notifications.length > 0 ? (
          [...notifications]
        .sort((a, b) =>
          a.isRead === b.isRead ? 0 : a.isRead ? 1 : -1
        )
        .map((n) => (
          <MenuItem
            key={n.id}
            sx={{
          display: "flex",
          flexDirection: "column",
          justifyContent: "space-between",
          alignItems: "flex-start",
          backgroundColor: !n.isRead ? "action.selected" : "transparent",
            }}
          >
            <Typography
          variant="body2"
          sx={{
            fontWeight: !n.isRead ? "bold" : "normal",
            mr: 1,
            flex: 1,
          }}
            >
          {n.content}
            </Typography>
            <Box
          display="flex"
          justifyContent="space-between"
          alignItems="center"
          gap={2}
            >
          <Typography
            variant="body2"
            sx={{
              whiteSpace: "nowrap",
              fontSize: "0.7rem",
              color: "text.secondary",
            }}
          >
            {formatDateTime(n.createdDate.toString())}
          </Typography>
          <Button
            size="small"
            variant="outlined"
            color="primary"
            disabled={n.isRead}
            onClick={() => handleNotificationClick(n.id)}
          >
            {n.isRead ? "Đã đọc" : "Đánh dấu đã đọc"}
          </Button>
            </Box>
          </MenuItem>
        ))
        ) : (
          <MenuItem onClick={handleClose} sx={{ p: 1 }}>
        <Typography variant="body2">
          Không có thông báo nào để hiển thị.
        </Typography>
          </MenuItem>
        )}
      </Menu>
    </>
  );
}
