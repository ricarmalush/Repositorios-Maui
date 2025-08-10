using Newtonsoft.Json;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Transference.Dtos;
using Transference.Endpoints;
using Transference.Excepciones;
using Transference.Interface;
using Transference.Models;
using Transference.Resources.Bases.Response;
using Transference.Shared;

namespace Transference.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _client;

        public ApiService(HttpClient client)
        {
            _client = client;
            _client.Timeout = TimeSpan.FromSeconds(10);
        }

        public async Task<bool> LoginAsync(string usuario, string password, string authType)
        {
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException(DiccionaryMessage.Empty);

            var loginModel = new { Usuario = usuario, Password = password };
            var content = new StringContent(JsonConvert.SerializeObject(loginModel), Encoding.UTF8, "application/json");
            var url = $"{Base.BaseUrl2}{Endpoint.Login}?authType={authType}";

            try
            {
                var response = await _client.PostAsync(url, content);
                response.EnsureSuccessStatusCode(); // Lanzará excepción si el estado no es 2xx

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenDto = JsonConvert.DeserializeObject<TokenDto>(jsonResponse);

                if (string.IsNullOrEmpty(tokenDto?.Token))
                {
                    throw new LoginException(DiccionaryMessage.TokenError);
                }

                await SecureStorage.SetAsync("auth_token", tokenDto.Token);
                SetAuthorizationToken(tokenDto.Token); // Establecemos el token
                return true;
            }
            catch (HttpRequestException ex)
            {
                throw new LoginException($"{DiccionaryMessage.ErrorSesion} {ex.Message}");
            }
        }
        // Método para obtener el token
        private async Task<string> GetTokenAsync()
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
            {
                await Application.Current.MainPage.DisplayAlert("Token", DiccionaryMessage.TokenNotFound, "OK");
            }
            return token; // Puede ser null si el token no existe
        }

        public async Task<string> GetActualStateAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException(DiccionaryMessage.InvalidUserId);

            var url = $"{Base.BaseUrl2}{Endpoint.ActualState}?userId={userId}";

            try
            {
                var token = await GetTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }

                var response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Lanzará excepción si el estado no es 2xx

                var jsonResponse = await response.Content.ReadAsStringAsync();

                // Usamos JsonDocument para leer el valor de "estadoActual"
                using var document = JsonDocument.Parse(jsonResponse);
                var estadoActual = document.RootElement.GetProperty("estadoActual").GetString();

                return estadoActual;
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"{DiccionaryMessage.ErrorRetrievingState}: {ex.Message}");
            }
        }

        public void SetAuthorizationToken(string token)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<ProvComResponseDto> GetProvComIDAsync(ProvComRequestDto provComRequestDto)
        {
            // Serializar el request
            var content = new StringContent(JsonConvert.SerializeObject(provComRequestDto), Encoding.UTF8, "application/json");
            var url = $"{Base.BaseUrl1}{Endpoint.AppProvComm}";

            try
            {
                // Realizar la solicitud HTTP
                var response = await _client.PostAsync(url, content);

                // Verificar si la solicitud fue exitosa
                response.EnsureSuccessStatusCode(); // Lanza excepción si el estado no es 2xx

                // Leer y deserializar la respuesta JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProvComResponseDto>(jsonResponse);

                // Verificar si la deserialización fue exitosa
                if (result != null)
                    return result;
                else
                    throw new Exception("La respuesta no pudo ser deserializada correctamente.");
            }
            catch (HttpRequestException ex)
            {
                // Manejo de la excepción en caso de un problema de red
                throw new Exception("Error al realizar la solicitud HTTP: " + ex.Message);
            }
            catch (JsonSerializationException ex)
            {
                // Manejo de la excepción en caso de un problema de deserialización
                throw new Exception("Error al deserializar la respuesta JSON: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de cualquier otra excepción
                throw new Exception("Ocurrió un error inesperado: " + ex.Message);
            }
        }


        //public async Task<SigningUpModel> GetActualStateAsync(SigningUpModel signingUpModel, string TypeSigning)
        //{
        //    try
        //    {
        //        var requestLocation = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
        //        var location = await Geolocation.GetLocationAsync(requestLocation);
        //        var details = await GetLocationDetailsAsync(location.Latitude, location.Longitude).ConfigureAwait(false);
        //        var provComID = await GetProvComIDAsync(new ProvComRequestDto
        //        {
        //            NameProvince = details.Address.Province ?? string.Empty,
        //            NameCommunity = details.Address.Town ?? string.Empty
        //        });
        //        string userId = ReturnIdUserToken.GetUserIdFromToken(await GetTokenAsync());


        //        // Actualiza los valores de SigningUpModel con los valores necesarios
        //        signingUpModel.UserId = int.Parse(userId);
        //        signingUpModel.Latitude = location.Latitude;
        //        signingUpModel.Longitude = location.Longitude;
        //        signingUpModel.SigningDateTime = DateTime.UtcNow; // Obtiene la fecha y hora actual
        //        signingUpModel.ClockCreateBy = "UsuarioApp";  // Valor predeterminado para este campo
        //        signingUpModel.Type = TypeSigning;
        //        signingUpModel.Street = details.Address.Road;

        //        signingUpModel.ProvinceId = provComID.ProvinceID;
        //        signingUpModel.CommunityId = provComID.CommunityID;
        //        signingUpModel.Province = details.Address.Province;
        //        signingUpModel.Community = details.Address.Town;

        //        await RegisterSigning(signingUpModel);

        //        //Guardar datos.
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }

        //    return signingUpModel;
        //}

        public async Task<SigningUpModel> GetActualStateAsync(SigningUpModel signingUpModel, string TypeSigning)
        {
            try
            {
                // Verificar si los permisos necesarios están otorgados
                var permissionStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                if (permissionStatus != PermissionStatus.Granted)
                {
                    var requestStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    if (requestStatus != PermissionStatus.Granted)
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "Permiso requerido",
                            "La app necesita permisos de ubicación para continuar.",
                            "OK"
                        );
                        return null; // Devuelve nulo si el usuario no otorga los permisos
                    }
                }

                // Solicitar la ubicación
                var requestLocation = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                var location = await Geolocation.GetLocationAsync(requestLocation);

                if (location == null)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        "Error de ubicación",
                        "No se pudo obtener la ubicación actual. Inténtalo nuevamente.",
                        "OK"
                    );
                    return null;
                }

                // Obtener detalles de ubicación y otros datos
                var details = await GetLocationDetailsAsync(location.Latitude, location.Longitude).ConfigureAwait(false);
                var provComID = await GetProvComIDAsync(new ProvComRequestDto
                {
                    NameProvince = details.Address.Province ?? string.Empty,
                    NameCommunity = details.Address.Town ?? string.Empty
                });
                string userId = ReturnIdUserToken.GetUserIdFromToken(await GetTokenAsync());

                // Actualiza los valores de SigningUpModel
                signingUpModel.UserId = int.Parse(userId);
                signingUpModel.Latitude = location.Latitude;
                signingUpModel.Longitude = location.Longitude;
                signingUpModel.SigningDateTime = DateTime.UtcNow; // Obtiene la fecha y hora actual
                signingUpModel.ClockCreateBy = "UsuarioApp";  // Valor predeterminado para este campo
                signingUpModel.Type = TypeSigning;
                signingUpModel.Street = details.Address.Road;

                signingUpModel.ProvinceId = provComID.ProvinceID;
                signingUpModel.CommunityId = provComID.CommunityID;
                signingUpModel.Province = details.Address.Province;
                signingUpModel.Community = details.Address.Town;

                await RegisterSigning(signingUpModel); // Registrar los datos
            }
            catch (FeatureNotEnabledException)
            {
                // Caso cuando los servicios de ubicación están desactivados
                bool openSettings = await Application.Current.MainPage.DisplayAlert(
                    "Ubicación desactivada",
                    "La ubicación está desactivada. ¿Deseas habilitarla en la configuración?",
                    "Sí",
                    "No"
                );

                if (openSettings)
                {
                    AppInfo.ShowSettingsUI(); // Abre la configuración de ubicación
                }
                return null;
            }
            catch (Exception ex)
            {
                // Manejar cualquier excepción
                await Application.Current.MainPage.DisplayAlert(
                    "Error",
                    $"Ocurrió un error al intentar obtener la ubicación: {ex.Message}",
                    "OK"
                );
                return null;
            }

            return signingUpModel;
        }


        public async Task<NominatimResponse> GetLocationDetailsAsync(double latitude, double longitude)
        {
            using (var httpClient = new HttpClient())
            {
                // Convertir a string con cultura invariante para evitar problemas de formato
                var latitudeString = latitude.ToString(CultureInfo.InvariantCulture);
                var longitudeString = longitude.ToString(CultureInfo.InvariantCulture);

                var url = $"https://nominatim.openstreetmap.org/reverse?lat={latitudeString}&lon={longitudeString}&format=json";

                try
                {
                    var jsonResponse = await httpClient.GetStringAsync(url); // Obtiene la respuesta JSON

                    // Deserializa la respuesta JSON en el modelo
                    var locationDetails = JsonConvert.DeserializeObject<NominatimResponse>(jsonResponse);

                    return locationDetails; // Devuelve locationDetails deserializado
                }
                catch (HttpRequestException httpRequestException)
                {
                    Console.WriteLine("Error de solicitud HTTP: " + httpRequestException.Message);
                    return null; // O maneja de otra manera
                }
                catch (System.Text.Json.JsonException jsonException)
                {
                    Console.WriteLine("Error de deserialización: " + jsonException.Message);
                    return null; // O maneja de otra manera
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    return null; // O maneja de otra manera
                }
            }
        }

        public async Task<BaseResponse<bool>> RegisterSigning(SigningUpModel requestDto)
        {
            // Serializar el request
            var content = new StringContent(JsonConvert.SerializeObject(requestDto), Encoding.UTF8, "application/json");
            var url = $"{Base.BaseUrl2}{Endpoint.SigningUpRegister}";

            try
            {
                // Realizar la solicitud HTTP
                var response = await _client.PostAsync(url, content);

                // Verificar si la solicitud fue exitosa
                response.EnsureSuccessStatusCode(); // Lanza excepción si el estado no es 2xx

                // Leer y deserializar la respuesta JSON
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ProvComResponseDto>(jsonResponse);

                if (result != null)
                {
                    // Devolver BaseResponse con éxito
                    return new BaseResponse<bool>
                    {
                        IsSuccess = true,
                        Message = "Registro exitoso.",
                        Data = true
                    };
                }
                else
                {
                    // En caso de que no haya resultado válido
                    return new BaseResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "El resultado de la respuesta fue nulo.",
                        Data = false
                    };
                }
            }
            catch (HttpRequestException httpRequestException)
            {
                // Manejo de excepciones de HTTP
                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Message = $"Error en la solicitud HTTP: {httpRequestException.Message}",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                // Manejo de cualquier otra excepción
                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Message = $"Ocurrió un error inesperado: {ex.Message}",
                    Data = false
                };
            }
        }
        
        public async Task<BaseResponse<bool>> GetOverTime(int userId)
        {
            if (userId <= 0)
            {
                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = DiccionaryMessage.InvalidUserId
                };
            }

            var url = BuildUrl(userId);  

            try
            {
                var token = await GetTokenAsync();

                if (!string.IsNullOrEmpty(token))
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _client.GetAsync(url);

                response.EnsureSuccessStatusCode(); // Lanza excepción si el estado no es 2xx

                var responseContent = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<BaseResponse<List<OvertimeDetails>>>(responseContent);

                if (result == null)
                {
                    return new BaseResponse<bool>
                    {
                        IsSuccess = false,
                        Data = false,
                        Message = "La respuesta deserializada es nula"
                    };
                }

                bool data = result.Data?.FirstOrDefault()?.Property1 != null; // Verifica la primera propiedad


                return new BaseResponse<bool>
                {
                    IsSuccess = result.IsSuccess,
                    Data = data,
                    Message = result.Message,
                    Errors = result.Errors
                };
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HttpRequestException: {ex.Message}");
                await Application.Current.MainPage.DisplayAlert("Error", $"{DiccionaryMessage.ErrorRetrievingState}: {ex.Message}", "OK");

                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = ex.Message
                };
            }
            catch (TaskCanceledException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Tiempo de espera agotado al intentar obtener los datos.", "OK");

                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = "Tiempo de espera agotado"
                };
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Se produjo un error inesperado: {ex.Message}", "OK");

                return new BaseResponse<bool>
                {
                    IsSuccess = false,
                    Data = false,
                    Message = ex.Message
                };
            }
        }

        private string BuildUrl(int userId)
        {
            return $"{Base.BaseUrl2}{Endpoint.OverTime}?userId={userId}";
        }

    }
}

