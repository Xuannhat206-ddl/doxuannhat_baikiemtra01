
using System;

public class OTo : PhuongTien
{
    private int _soChoNgoi;
    private double _dungTichDongCo;

    public int SoChoNgoi
    {
        get { return _soChoNgoi; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("So cho phai lon hon 0!");

            _soChoNgoi = value;
        }
    }

    public double DungTichDongCo
    {
        get { return _dungTichDongCo; }
        set
        {
            if (value <= 0)
                throw new ArgumentException("Dung tich phai lon hon 0!");

            _dungTichDongCo = value;
        }
    }

    public OTo(string maPT, string tenHang, int namSanXuat,
               decimal giaGoc, int soChoNgoi, double dungTichDongCo)
        : base(maPT, tenHang, namSanXuat, giaGoc)
    {
        SoChoNgoi = soChoNgoi;
        DungTichDongCo = dungTichDongCo;
    }

    public override decimal TinhGiaLanBanh()
    {
        if (SoChoNgoi <= 9)
            return GiaGoc + GiaGoc * 0.12m + GiaGoc * 0.30m;
        else
            return GiaGoc + GiaGoc * 0.10m;
    }

    public override string GetInfo()
    {
        return base.GetInfo()
            + ", So cho: " + SoChoNgoi
            + ", Dung tich dong co: " + DungTichDongCo;
    }
}