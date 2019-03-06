# Data Specification

Version `0.2`

## Notes

- All white cells are confirmed for this version. To suggest changes, use comments.
- All datetime fields should be recorded as a string formatted according to the ISO 8601 standard. An example of this would be “2018-02-26T21:18:04”.
- Session UUIDs should be created as a datetime field, with the UUID corresponding to the start of the session. All data relating to the same session should use the same UUID. Only the instantiation of a new session should generate a new datetime UUID.

## App-Generated Data

All set via the `App -> Account Settings` screen. **Except for username. Username is set on account creation.**

```
"user": {
    "username":     string (required, unique)   // set on account creation
    "name":         string (optional)           // app -> account settings
    "age":          float  (optional)           // app -> account settings
    "gender":       int    (optional, m=0/f=1)  // app -> account settings
    "ethnicity":    string (optional)           // app -> account settings
    "location":     string (optional)           // app -> account settings
    "occupation":   string (optional)           // app -> account settings
}
```

Created from the `App -> Logging` screen.

```
"selfReported": {
    "sessionid":    string              // ISO 8601 compliant
    "anxiety":      float normalised
    "stress":       float normalised
    "fatigue":      float normalised
    "productivity": float normalised
}
```

## Hardware-Generated Data

```
<session_id>: {
    "firmwareRevision": (see BLE Spec),
    "ppg": { 
        "bodySensorLocation": (see BLE Spec),
        "heartRate": [
            <timestamp>: int (in BPM),
            ...
        ],
        "interbeatInterval": [
            <timestamp>: int (in BPM),
            ...
        ],
        "spO2": [
            <timestamp>: float (in %),
            ...
        ]
    }
    },
    "gsr": {
        "bodySensorLocation": (see BLE Spec),
        "scl": [
            <timestamp>: int (as adc value),
            ...
        ]
    }
}
```
