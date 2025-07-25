namespace ParkingLot.Service
{
    public static class GracePeriodService
    {
        public static bool IsWithinGracePeriod(DateTime checkIn, DateTime checkOut)
        {
            var duration = checkOut - checkIn;
            return duration.TotalMinutes <= 15;
        }
    }
}
