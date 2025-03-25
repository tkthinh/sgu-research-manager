import { format, parse } from 'date-fns';
import { vi } from 'date-fns/locale';

export const formatMonthYear = (date: Date | string | null | undefined): string => {
  if (!date) return '-';
  try {
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return format(dateObj, 'MM/yyyy', { locale: vi });
  } catch {
    return '-';
  }
};

export const parseMonthYear = (monthYear: string): Date | null => {
  try {
    // Chấp nhận cả định dạng MM/yyyy và MM-yyyy
    const normalizedDate = monthYear.replace('-', '/');
    return parse(normalizedDate, 'MM/yyyy', new Date());
  } catch {
    return null;
  }
};