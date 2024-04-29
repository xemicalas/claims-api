# ClaimsController

This controller manages operations related to claims.

## Endpoints

### Get All Claims
- **URL:** `/claims`
- **Method:** `GET`
- **Description:** Retrieves all available claims.
- **Returns:** A list of claims.

### Get Claim by ID
- **URL:** `/claims/{id}`
- **Method:** `GET`
- **Description:** Retrieves a specific claim by its identifier.
- **Parameters:**
  - `id`: Claim identifier
- **Returns:** The claim response.

### Create Claim
- **URL:** `/claims`
- **Method:** `POST`
- **Description:** Creates a new claim with the provided properties.
- **Body:** JSON object representing the claim.
- **Returns:** The created claim's identifier.
- **Possible Errors:**
  - 404 Not Found: If the provided cover is not found.

### Delete Claim
- **URL:** `/claims/{id}`
- **Method:** `DELETE`
- **Description:** Deletes a created claim by its identifier.
- **Parameters:**
  - `id`: Claim identifier
- **Returns:** No content.
- **Possible Errors:**
  - 404 Not Found: If the claim with the provided identifier is not found.


# CoversController

This controller manages operations related to covers.

## Endpoints

### Compute Premium
- **URL:** `/ComputePremium`
- **Method:** `POST`
- **Description:** Computes premium based on date intervals and cover type.
- **Parameters:**
  - `startDate`: Cover's start date (DateOnly format)
  - `endDate`: Cover's end date (DateOnly format)
  - `coverType`: Type of cover
- **Returns:** The calculated premium amount.

### Get All Covers
- **URL:** `/covers`
- **Method:** `GET`
- **Description:** Retrieves all created covers.
- **Returns:** A list of covers.

### Get Cover by ID
- **URL:** `/covers/{id}`
- **Method:** `GET`
- **Description:** Retrieves a specific cover by its identifier.
- **Parameters:**
  - `id`: Cover identifier
- **Returns:** The cover object.

### Create Cover
- **URL:** `/covers`
- **Method:** `POST`
- **Description:** Creates a new cover with the provided properties.
- **Body:** JSON object representing the cover.
- **Returns:** The created cover's identifier.

### Delete Cover
- **URL:** `/covers/{id}`
- **Method:** `DELETE`
- **Description:** Deletes a created cover by its identifier.
- **Parameters:**
  - `id`: Cover identifier
- **Returns:** No content.