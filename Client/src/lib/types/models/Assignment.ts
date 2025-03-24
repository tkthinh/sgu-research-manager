export interface Assignment {
  id: string;
  managerId: string;
  departmentId: string;
  managerFullName?: string;
  managerDepartmentName?: string;
  assignedDepartmentName?: string;
}