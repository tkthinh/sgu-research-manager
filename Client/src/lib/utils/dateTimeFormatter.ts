import { DateTime } from "luxon";

export function formatDateTime(dateString: string): string {
  // Parse the ISO date string and set locale to Vietnamese
  const dt = DateTime.fromISO(dateString).setLocale("vi");
  // Format the date to mimic the native options:
  // Day as numeric, full month name, year numeric, and time (24hr) in "HH:mm".
  return dt.toFormat("d LLLL yyyy, HH:mm");
}
