(function () {
    function init() {
        var input = document.getElementById('XperienceAddressInput');
        var dropdown = document.getElementById('XperienceAddressAutocompleteDropdown');
        var service = new google.maps.places.AutocompleteService();

        var currentLocationButton = document.createElement('div');
        currentLocationButton.textContent = "Current Location";

        currentLocationButton.addEventListener('mousedown', function () {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(function (position) {
                    var geolocation = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };
                    var geocoder = new google.maps.Geocoder;
                    geocoder.geocode({ 'location': geolocation }, function (results, status) {
                        if (status === 'OK') {
                            if (results[0]) {
                                input.value = results[0].formatted_address;
                            } else {
                                window.alert('No results found');
                            }
                        } else {
                            window.alert('Geocoder failed due to: ' + status);
                        }
                    });
                });
            }
            dropdown.style.display = 'none';
        });

        function startAutocomplete() {
            service.getPlacePredictions({ input: input.value, componentRestrictions: { country: 'cz' } }, function (predictions, status) {
                if (status === google.maps.places.PlacesServiceStatus.OK) {
                    dropdown.innerHTML = '';
                    dropdown.appendChild(currentLocationButton);
                    predictions.forEach(function (prediction) {
                        var item = document.createElement('div');
                        item.textContent = prediction.description;
                        item.addEventListener('mousedown', function () {
                            input.value = prediction.description;
                            dropdown.style.display = 'none';
                        });
                        dropdown.appendChild(item);
                    });
                    dropdown.style.display = 'block';
                }
            });
        }

        input.addEventListener('input', function () {
            if (input.value.length > 0) {
                startAutocomplete();
            } else {
                dropdown.innerHTML = '';
                dropdown.appendChild(currentLocationButton);
                dropdown.style.display = 'block';
            }
        });

        input.addEventListener('focusin', function () {
            if (input.value.length === 0) {
                dropdown.innerHTML = '';
                dropdown.appendChild(currentLocationButton);
                dropdown.style.display = 'block';
            } else {
                startAutocomplete();
            }
        });

        input.addEventListener('focusout', function () {
            dropdown.style.display = 'none';
        });
    }
    if (document.readyState === "loading") {
        document.addEventListener("DOMContentLoaded", init);
    } else {
        init();
    }
}) ();