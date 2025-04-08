import { DateTime } from "luxon";

export const isSystemOpen = (config) => {
  // Let's assume your openTime and closeTime are in ISO format (UTC)
  // Convert them to the desired time zone if needed (e.g., Asia/Saigon).
  const now = DateTime.now().setZone("Asia/Saigon");
  const openDateTime = DateTime.fromISO(config.openTime, { zone: "utc" }).setZone("Asia/Saigon");
  const closeDateTime = DateTime.fromISO(config.closeTime, { zone: "utc" }).setZone("Asia/Saigon");
  
  // Compare current time with the system's open and close times.
  return now >= openDateTime && now <= closeDateTime;
};
