<h2 align="center"><br>Data Specification</h2>

<p align="center">
Version <code>0.4</code>
</p>
<br>

## User Config Data

All set via the `App -> Account Settings` screen. **Except for username. Username is set on account creation.**

```
"username": {
    "username":     string (required, unique)   // set on account creation
    "name":         string (optional)           // app -> account settings
    "age":          float  (optional)           // app -> account settings
    "gender":       int    (optional, m=0/f=1)  // app -> account settings
    "ethnicity":    string (optional)           // app -> account settings
    "location":     string (optional)           // app -> account settings
    "occupation":   string (optional)           // app -> account settings
}
```

## Session Data

- `session_id` is a datetime string and should be formatted according to the ISO 8601 standard e.g. `“2018-02-26T21:18:04”`.
- `session_id` should be created at the start of the session. All data relating to the same session should use the same UUID. Only the instantiation of a new session should generate a new datetime UUID.
- `timestamp` is formatted as a unix timestamp. Resolution to the nearest second.

```
"session_id": {
    "session_id": string,                       // ISO 8601 compliant
    "firmwareRevision": (see BLE Spec),
    "selfReported": {                           // Generated from the `App -> Logging` screen
        "anxiety":      float normalised
        "stress":       float normalised
        "fatigue":      float normalised
        "productivity": float normalised
    },
    "ppg": {                                    // Generated from the hardware
        "bodySensorLocation": (see BLE Spec),
        "heartRate": {
            "timestamp": int (in BPM),
            ...
        },
        "interbeatInterval": {
            "timestamp": int (in BPM),
            ...
        },
        "spO2": {
            "timestamp": float (in %),
            ...
        }
    }
    },
    "gsr": {                                    // Generated from the hardware
        "bodySensorLocation": (see BLE Spec),
        "scl": {
            "timestamp": int (as adc value),
            ...
        }
    }
}
```
