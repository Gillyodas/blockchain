using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using ChainDegree.SharedKernel.QuanLyBangCap.CoSoDaoTao;
using ControlHub.Domain.Identity.ValueObjects;
using ControlHub.SharedKernel.Common.Errors;
using ControlHub.SharedKernel.Results;

namespace ChainDegree.Domain.QuanLyBangCap.Entities;

public class SinhVien
{
    public Guid Id { get; private set; }
    public string Ten { get; private set; }
    public string CCCD { get; private set; }
    public Email Email { get; private set; }
    public string DiaChiViSinhVien { get; private set; }
    public Guid TKId { get; private set; }
    public DateTime ThoiGianTao { get; private set; } = DateTime.Now;
    public DateTime? ThoiGianCapNhat { get; private set; }
    public DateTime? ThoiGianXoa { get; private set; }

    private static readonly Regex _cccdRegex =
        new(@"/^0\d{2}[0-9]\d{2}\d{6}$/",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private SinhVien(Guid id, string ten, string cccd, Email email, string diaChiViSinhVien, Guid tkId)
    {
        Id = id;
        Ten = ten;
        CCCD = cccd;
        Email = email;
        DiaChiViSinhVien = diaChiViSinhVien;
        TKId = tkId;
    }

    internal static Result<SinhVien> Create(string ten, string cccd, string email, string diaChiViSinhVien, Guid tkId)
    {
        if(_cccdRegex.IsMatch(cccd) == false)
        {
            return Result<SinhVien>.Failure(CoSoDaoTaoError.SaiDinhDangCCCD);
        }

        Result<Email> resultCreateEmail = Email.Create(email);
        if(resultCreateEmail.IsFailure)
        {
            return Result<SinhVien>.Failure(CoSoDaoTaoError.SaiDinhDangEmail);
        }

        SinhVien sinhVien = new SinhVien(Guid.NewGuid(), ten, cccd, resultCreateEmail.Value, diaChiViSinhVien, tkId);

        return Result<SinhVien>.Success(sinhVien);
    }

    
}
