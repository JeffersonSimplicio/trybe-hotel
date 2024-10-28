using System.Net.Http;
using System.Runtime.ConstrainedExecution;
using TrybeHotel.Dto;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Services;
public class GeoService : IGeoService {
    private readonly HttpClient _client;
    public GeoService(HttpClient client) {
        _client = client;
        _client.DefaultRequestHeaders.Add("Accept", "application/json");
        _client.DefaultRequestHeaders.Add("User-Agent", "aspnet-user-agent");
    }

    // 11. Desenvolva o endpoint GET /geo/status
    public async Task<object> GetGeoStatus() {
        string url = "https://nominatim.openstreetmap.org/status.php?format=json";

        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            url
        );

        var response = await _client.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode) {
            throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<object>();
        return result!;
    }

    // 12. Desenvolva o endpoint GET /geo/address
    public async Task<GeoDtoResponse> GetGeoLocation(GeoDto geoDto) {
        string url = $"https://nominatim.openstreetmap.org/search?street={geoDto.Address}&city={geoDto.City}&country=Brazil&state={geoDto.State}&format=json&limit=1";

        var requestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            url
        );

        var response = await _client.SendAsync(requestMessage);

        if (!response.IsSuccessStatusCode) {
            throw new HttpRequestException($"Request to {url} failed with status code {response.StatusCode}");
        }

        var result = await response.Content.ReadFromJsonAsync<List<GeoDtoResponse>>();
        return new GeoDtoResponse() {
            lat = result![0].lat,
            lon = result[0].lon,
        };
    }

    // 12. Desenvolva o endpoint GET /geo/address
    public async Task<List<GeoDtoHotelResponse>> GetHotelsByGeo(GeoDto geoDto, IHotelRepository repository) {
        GeoDtoResponse userLocal = await GetGeoLocation(geoDto);
        var listHotels = repository.GetAllHotels();

        var hotelsWithDistance = await Task.WhenAll(listHotels.Select(async hotel => {
            GeoDtoResponse hotelLocal = await GetGeoLocation(new GeoDto() {
                Address = hotel.Address,
                City = hotel.cityName,
                State = hotel.state,
            });

            int distance = CalculateDistance(
                userLocal.lat, userLocal.lon,
                hotelLocal.lat, hotelLocal.lon
            );

            return new GeoDtoHotelResponse() {
                HotelId = hotel.HotelId!.Value,
                Name = hotel.Name,
                Address = hotel.Address,
                CityName = hotel.cityName,
                State = hotel.state,
                Distance = distance,
            };
        }));

        return hotelsWithDistance.ToList();
    }

    public int CalculateDistance(string latitudeOrigin, string longitudeOrigin, string latitudeDestiny, string longitudeDestiny) {
        double latOrigin = double.Parse(latitudeOrigin.Replace('.', ','));
        double lonOrigin = double.Parse(longitudeOrigin.Replace('.', ','));
        double latDestiny = double.Parse(latitudeDestiny.Replace('.', ','));
        double lonDestiny = double.Parse(longitudeDestiny.Replace('.', ','));
        double R = 6371;
        double dLat = radiano(latDestiny - latOrigin);
        double dLon = radiano(lonDestiny - lonOrigin);
        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(radiano(latOrigin)) * Math.Cos(radiano(latDestiny)) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        double distance = R * c;
        return int.Parse(Math.Round(distance, 0).ToString());
    }

    public double radiano(double degree) {
        return degree * Math.PI / 180;
    }

}