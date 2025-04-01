import { User } from "../types/models/User";

export function isUserProfileIncomplete(user: User): boolean {
  const invalidStrings = ["-", "", "Unknown"];
  const emptyGuid = "00000000-0000-0000-0000-000000000000";

  return (
    invalidStrings.includes(user.email) ||
    invalidStrings.includes(user.phoneNumber) ||
    invalidStrings.includes(user.specialization) ||
    invalidStrings.includes(user.academicTitle) ||
    invalidStrings.includes(user.officerRank) ||
    invalidStrings.includes(user.fieldName) ||
    user.fieldId === emptyGuid
  );
}
