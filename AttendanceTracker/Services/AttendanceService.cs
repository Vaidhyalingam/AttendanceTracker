using Microsoft.JSInterop;
using System.Text.Json;

namespace AttendanceTracker.Services;

public class AttendanceService
{
    private readonly IJSRuntime _jsRuntime;
    private const int RequiredDays = 12;
    private List<DateTime> AttendanceRecords = new();

    public AttendanceService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task LoadAttendanceAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "attendance");
        if (!string.IsNullOrEmpty(json))
        {
            AttendanceRecords = JsonSerializer.Deserialize<List<DateTime>>(json) ?? new List<DateTime>();
        }
    }

    public async Task MarkAttendanceAsync()
    {
        var today = DateTime.UtcNow.AddHours(5.5).Date;
        if (!AttendanceRecords.Contains(today))
        {
            AttendanceRecords.Add(today);
            await SaveAttendanceAsync();
        }
    }

    private async Task SaveAttendanceAsync()
    {
        var json = JsonSerializer.Serialize(AttendanceRecords);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "attendance", json);
    }

    public int GetAttendanceCount()
    {
        var currentMonth = DateTime.UtcNow.AddHours(5.5).Month;
        return AttendanceRecords.Count(t => t.Month == currentMonth);
    }

    public double GetProgress() => (double)GetAttendanceCount() / RequiredDays * 100;
    public bool IsCompleted() => GetAttendanceCount() >= RequiredDays;

    public List<DateTime> GetAttendanceRecords() => AttendanceRecords
        .Where(t => t.Month == DateTime.UtcNow.AddHours(5.5).Month)
        .ToList();
}