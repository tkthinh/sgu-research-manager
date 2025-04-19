export type FieldDef = { key: string; label: string; required: boolean };

export interface TypeConfig {
  default: FieldDef[];
  [levelName: string]: FieldDef[];
}

export const workDetailsConfig: Record<string, TypeConfig> = {
  // =================== BÀI BÁO KHOA HỌC ===================
  "bài báo khoa học": {
    WoS: [
      {
        key: "Tên tạp chí khoa học",
        label: "Tên tạp chí khoa học",
        required: true,
      },
      {
        key: "Tên tạp chí khoa học",
        label: "Tên tạp chí khoa học",
        required: true,
      },
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Loại WoS", label: "Loại WoS", required: true },
      { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: true },
    ],
    Scopus: [
      {
        key: "Tên tạp chí khoa học",
        label: "Tên tạp chí khoa học",
        required: true,
      },
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: true },
    ],
    // Cấp trường, bộ/ngành, quốc tế
    default: [
      {
        key: "Tên tạp chí khoa học",
        label: "Tên tạp chí khoa học",
        required: true,
      },
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: true },
    ],
  },
  // =================== BÁO CÁO KHOA HỌC ===================
  "báo cáo khoa học": {
    WoS: [
      {
        key: "Tên ấn phẩm",
        label: "Tên ấn phẩm",
        required: true,
      },
      {
        key: "Tên ấn phẩm",
        label: "Tên ấn phẩm",
        required: true,
      },
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Loại WoS", label: "Loại WoS", required: true },
      { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: true },
    ],
    Scopus: [
      {
        key: "Tên ấn phẩm",
        label: "Tên ấn phẩm",
        required: true,
      },
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Xếp hạng tạp chí", label: "Xếp hạng tạp chí", required: true },
    ],
    // Cấp trường, bộ/ngành, quốc tế
    default: [
      {
        key: "Tên ấn phẩm",
        label: "Tên ấn phẩm",
        required: true,
      },
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quản xuất bản", label: "Cơ quan xuất bản", required: true },
    ],
  },
  // =================== ĐỀ TÀI ===================
  "đề tài": {
    default: [
      { key: "Mã số đề tài", label: "Mã số đề tài", required: true },
      {
        key: "Sản phẩm thuộc đề tài",
        label: "Sản phẩm thuộc đề tài",
        required: true,
      },
      { key: "Xếp loại", label: "Xếp loại", required: true },
    ],
  },
  // =================== GIÁO TRÌNH ===================
  "giáo trình": {
    default: [
      { key: "Mã số giáo trình", label: "Mã số giáo trình", required: true },
      { key: "Xếp loại", label: "Xếp loại", required: true },
    ],
  },
  // =================== TÀI LIỆU GIẢNG DẠY ===================
  "tài liệu giảng dạy": {
    default: [
      { key: "Mã số tài liệu", label: "Mã số tài liệu", required: true },
      { key: "Xếp loại", label: "Xếp loại", required: true },
    ],
  },
  // =================== HỘI THẢO, HỘI NGHỊ ===================
  "hội thảo, hội nghị": {
    default: [{ key: "Chi tiết", label: "Chi tiết", required: true }],
  },
  // =================== HD SV NCKH ===================
  "hướng dẫn sv nckh": {
    default: [
      {
        key: "Mã số đề tài của sinh viên",
        label: "Mã số đề tài của sinh viên",
        required: true,
      },
    ],
  },
  // =================== SÁCH CÁC LOẠI ===================
  "chương sách": {
    default: [
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quan xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Dạng tài liệu", label: "Dạng tài liệu", required: true },
    ],
  },
  "chuyên khảo": {
    default: [
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quan xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Dạng tài liệu", label: "Dạng tài liệu", required: true },
    ],
  },
  "giáo trình - sách": {
    default: [
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quan xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Dạng tài liệu", label: "Dạng tài liệu", required: true },
    ],
  },
  "tài liệu hướng dẫn": {
    default: [
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quan xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Dạng tài liệu", label: "Dạng tài liệu", required: true },
    ],
  },
  "tham khảo": {
    default: [
      { key: "Tập, số phát hành", label: "Tập, số phát hành", required: true },
      { key: "Số trang", label: "Số trang", required: true },
      { key: "Chỉ số xuất bản", label: "Chỉ số xuất bản", required: true },
      { key: "Cơ quan xuất bản", label: "Cơ quan xuất bản", required: true },
      { key: "Dạng tài liệu", label: "Dạng tài liệu", required: true },
    ],
  },
  khác: {
    default: [{ key: "Chi tiết", label: "Chi tiết", required: true }],
  },
};
