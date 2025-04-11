import { DateTime } from "luxon";

export function formatDateTime(dateString: string): string {
  // Parse the ISO date string from UTC.
  const dtUTC = DateTime.fromISO(dateString, { zone: "utc" });
  // Convert the time to "Asia/Saigon" timezone and set locale to Vietnamese.
  const dtLocal = dtUTC.setZone("Asia/Saigon").setLocale("vi");
  // Format the date to mimic the native options:
  // Day as numeric, full month name, year numeric, and time (24hr) in "HH:mm".
  return dtLocal.toFormat("d LLLL yyyy, HH:mm");
}
