function initializeAutocomplete(
    inputId,
    dropdownId,
    logoItemId,
    supportedCountries,
    suggestionsLanguage,
    currentLocationButtonLabel,
    enableCurrentLocationSuggestions,
    enableCompanyNames
) {
    const input = document.getElementById(inputId);
    const dropdown = document.getElementById(dropdownId);
    const currentLocationButton = enableCurrentLocationSuggestions ? getCurrentLocationButton() : undefined;
    let activeItemIndex = -1;

    input.addEventListener('input', function () {
        if (input.value.length > 0) {
            getSuggestions();
        } else {
            showDropdown();
            const logoItem = createLogoItem();
            dropdown.appendChild(logoItem);
        }
        activeItemIndex = -1;
    });

    input.addEventListener('focusin', function () {
        if (input.value.length === 0) {
            showDropdown();
            const logoItem = createLogoItem();
            dropdown.appendChild(logoItem);
        } else {
            getSuggestions();
        }
        activeItemIndex = -1;
    });

    input.addEventListener('focusout', function () {
        hideDropdown();
    });

    input.addEventListener('keydown', function (e) {
        const items = Array.from(dropdown.childNodes).filter((i) => i.id !== logoItemId);
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
                hideDropdown();
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
                    const geolocation = {
                        lat: position.coords.latitude,
                        lng: position.coords.longitude
                    };

                    const geocoder = new google.maps.Geocoder();
                    geocoder.geocode({ location: geolocation }, function (results, status) {
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

            hideDropdown();
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
                    showDropdown();

                    for (const prediction of predictions) {
                        const item = createDropdownItem(prediction.description);

                        item.addEventListener('mousedown', function () {
                            input.value = item.textContent;
                            hideDropdown();
                        });

                        dropdown.appendChild(item);
                    }

                    const logoItem = createLogoItem();
                    dropdown.appendChild(logoItem);
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

    function createLogoItem() {
        const logoItem = document.createElement('div');
        logoItem.innerHTML = '<img src="https://maps.gstatic.com/mapfiles/api-3/images/powered-by-google-on-white3.png" />';
        logoItem.className = 'xperience-address-dropdown-item-logo';
        logoItem.id = logoItemId;
        return logoItem;
    }
}
