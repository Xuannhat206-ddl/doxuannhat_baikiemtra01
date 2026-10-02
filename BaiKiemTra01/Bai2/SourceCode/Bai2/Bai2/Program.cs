
using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        QuanLyPhuongTien ql = new QuanLyPhuongTien();

        Console.WriteLine("=== QUAN LY PHUONG TIEN AUTOSPEED ===");
        Console.WriteLine("1. Them o to");
        Console.WriteLine("2. Them xe may");
        Console.WriteLine("3. Hien thi danh sach");
        Console.WriteLine("4. Tim gia lan banh cao nhat");
        Console.WriteLine("5. Tim theo ten hang");
        Console.WriteLine("0. Thoat");

        int chon;

        do
        {
            Console.Write("\nNhap lua chon: ");
            chon = int.Parse(Console.ReadLine());

            try
            {
                switch (chon)
                {
                    case 1:
                        {
                            Console.Write("Nhap ma phuong tien: ");
                            string ma = Console.ReadLine();

                            Console.Write("Nhap ten hang: ");
                            string ten = Console.ReadLine();

                            Console.Write("Nhap nam san xuat: ");
                            int nam = int.Parse(Console.ReadLine());

                            Console.Write("Nhap gia goc: ");
                            decimal gia = decimal.Parse(Console.ReadLine());

                            Console.Write("Nhap so cho ngoi: ");
                            int soCho = int.Parse(Console.ReadLine());

                            Console.Write("Nhap dung tich dong co: ");
                            double dungTich = double.Parse(Console.ReadLine());

                            OTo oto = new OTo(
                                ma, ten, nam, gia, soCho, dungTich);

                            ql.AddPhuongTien(oto);
                            Console.WriteLine("Them o to thanh cong!");
                            break;
                        }

                    case 2:
                        {
                            Console.Write("Nhap ma phuong tien: ");
                            string ma = Console.ReadLine();

                            Console.Write("Nhap ten hang: ");
                            string ten = Console.ReadLine();

                            Console.Write("Nhap nam san xuat: ");
                            int nam = int.Parse(Console.ReadLine());

                            Console.Write("Nhap gia goc: ");
                            decimal gia = decimal.Parse(Console.ReadLine());

                            Console.Write("Nhap dung tich xy lanh (cc): ");
                            int cc = int.Parse(Console.ReadLine());

                            XeMay xe = new XeMay(
                                ma, ten, nam, gia, cc);

                            ql.AddPhuongTien(xe);
                            Console.WriteLine("Them xe may thanh cong!");
                            break;
                        }

                    case 3:
                        ql.DisplayAll();
                        break;

                    case 4:
                        {
                            PhuongTien max = ql.FindMaxGiaLanBanh();

                            Console.WriteLine(max.GetInfo());
                            Console.WriteLine("Gia lan banh cao nhat: "
                                + max.TinhGiaLanBanh().ToString("N0")
                                + " VND");
                            break;
                        }

                    case 5:
                        {
                            Console.Write("Nhap ten hang can tim: ");
                            string keyword = Console.ReadLine();

                            List<PhuongTien> ketQua =
                                ql.SearchByName(keyword);

                            if (ketQua.Count == 0)
                            {
                                Console.WriteLine("Khong tim thay!");
                            }
                            else
                            {
                                foreach (PhuongTien pt in ketQua)
                                {
                                    Console.WriteLine(pt.GetInfo());
                                    Console.WriteLine("Gia lan banh: "
                                        + pt.TinhGiaLanBanh().ToString("N0")
                                        + " VND");
                                }
                            }

                            break;
                        }

                    case 0:
                        Console.WriteLine("Ket thuc chuong trinh!");
                        break;

                    default:
                        Console.WriteLine("Lua chon khong hop le!");
                        break;
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Loi: Du lieu nhap phai dung dinh dang!");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Loi du lieu: " + ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Thong bao: " + ex.Message);
            }

        } while (chon != 0);
    }
}