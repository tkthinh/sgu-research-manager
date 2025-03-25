export interface ConversionItem {
  totalWorks: number;
  totalConvertedHours: number;
  totalCalculatedHours: number;
}

export interface ConversionDetails {
  dutyHourConversion: ConversionItem;
  overLimitConversion: ConversionItem;
  researchProductConversion: ConversionItem;
  totalWorks: number;
  totalCalculatedHours: number;
}

export interface UserConversionResult {
  userId: string;
  userName: string;
  conversionResults: ConversionDetails;
} 