
using System;
using System.Collections.Generic;

public class QuanLyPhuongTien
{
    private List<PhuongTien> danhSach = new List<PhuongTien>();

    // 1. Them phuong tien
    public void AddPhuongTien(PhuongTien pt)
    {
        if (pt == null)
            throw new ArgumentException("Phuong tien khong hop le!");

        danhSach.Add(pt);
    }

    // 2. Hien thi danh sach
    public void DisplayAll()
    {
        foreach (PhuongTien pt in danhSach)
        {
            Console.WriteLine(pt.GetInfo());
            Console.WriteLine("Gia lan banh: "
                + pt.TinhGiaLanBanh().ToString("N0") + " VND");

            Console.WriteLine("--------------------------");
        }
    }

    // 3. Tim phuong tien co gia lan banh cao nhat
    public PhuongTien FindMaxGiaLanBanh()
    {
        if (danhSach.Count == 0)
            throw new InvalidOperationException("Danh sach rong!");

        PhuongTien max = danhSach[0];

        foreach (PhuongTien pt in danhSach)
        {
            if (pt.TinhGiaLanBanh() > max.TinhGiaLanBanh())
            {
                max = pt;
            }
        }

        return max;
    }

    // 4. Tim theo ten hang
    public List<PhuongTien> SearchByName(string keyword)
    {
        List<PhuongTien> ketQua = new List<PhuongTien>();

        foreach (PhuongTien pt in danhSach)
        {
            if (pt.TenHang.IndexOf(
                keyword ?? "", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                ketQua.Add(pt);
            }
        }

        return ketQua;
    }
}