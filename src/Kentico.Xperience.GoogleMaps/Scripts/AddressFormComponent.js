function initializeAutocomplete(inputId, dropdownId, supportedCountries, suggestionsLanguage, currentLocationButtonLabel, enableCurrentLocationSuggestions, enableCompanyNames) {
    const input = document.getElementById(inputId);
    const dropdown = document.getElementById(dropdownId);
    const currentLocationButton = enableCurrentLocationSuggestions ? getCurrentLocationButton(input, dropdown) : undefined;
    let activeItemIndex = -1;

    input.addEventListener('input', function () {
        if (input.value.length > 0) {
            getSuggestions(supportedCountries, input, dropdown, currentLocationButton);
        } else {
            showDropdown(dropdown, currentLocationButton);
        }
        activeItemIndex = -1;
    });

    input.addEventListener('focusin', function () {
        if (input.value.length === 0) {
            showDropdown(dropdown, currentLocationButton);
        } else {
            getSuggestions(supportedCountries, input, dropdown, currentLocationButton);
        }
        activeItemIndex = -1;
    });

    input.addEventListener('focusout', function () {
        dropdown.style.display = 'none';
    });

    input.addEventListener('keydown', function (e) {
        const items = dropdown.childNodes;
        if (e.key === 'ArrowDown') {
            activeItemIndex = (activeItemIndex + 1) % items.length;
        } else if (e.key === 'ArrowUp') {
            activeItemIndex = (activeItemIndex - 1 + items.length) % items.length;
        } else if (e.key === 'Enter') {
            e.preventDefault();
            if (activeItemIndex === 0 && enableCurrentLocationSuggestions) {
                currentLocationButton.dispatchEvent(new Event('mousedown'));
            } else if (activeItemIndex >= 0) {
                input.value = items[activeItemIndex].textContent;
                hideDropdown(dropdown);
            }
        }

        for (let i = 0; i < items.length; i++) {
            if (i === activeItemIndex) {
                items[i].classList.add('active');
            } else {
                items[i].classList.remove('active');
            }
        }
    });

    function getCurrentLocationButton() {
        const currentLocationButton = createDropdownItem(currentLocationButtonLabel);

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

            hideDropdown(dropdown);
        });

        return currentLocationButton;
    }

    function getSuggestions() {
        const autocompleteService = new google.maps.places.AutocompleteService();

        autocompleteService.getPlacePredictions(
            {
                input: input.value,
                componentRestrictions: supportedCountries ? { country: supportedCountries } : undefined,
                types: !enableCompanyNames ? ['address'] : undefined,
                language: suggestionsLanguage
            },
            function (predictions, status) {
                if (status === google.maps.places.PlacesServiceStatus.OK) {
                    showDropdown(dropdown, currentLocationButton);

                    for (prediction of predictions) {
                        const item = createDropdownItem(prediction.description);

                        item.addEventListener('mousedown', function () {
                            input.value = prediction.description;
                            hideDropdown(dropdown);
                        });

                        dropdown.appendChild(item);
                    }
                }
            }
        );
    }

    function showDropdown() {
        dropdown.innerHTML = '';
        if (enableCurrentLocationSuggestions) {
            dropdown.appendChild(currentLocationButton);
        }
        dropdown.style.display = 'block';
    }

    function hideDropdown() {
        dropdown.style.display = 'none';
    }

    function createDropdownItem(label) {
        const item = document.createElement('div');
        item.textContent = label;
        item.className = 'xperience-address-dropdown-item';

        return item;
    }
}