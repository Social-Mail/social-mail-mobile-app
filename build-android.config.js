import { readEnv } from "@neurospeech/jex/dist/index.js";
import { resolve } from "path";

export default [
    {
        id: "in.socialmail.app",
        name: "Social Mail",

        url: "https://mails.socialmail.in",

        targetFramework: "net9.0-android35.0",

        androidSdkRoot: readEnv("ANDROID_SDK_ROOT"),
        javaHome: readEnv("JAVA_HOME_21_X64", readEnv("JAVA_HOME")),

        androidKeyStore: resolve("./cert/android.keystore"),
        androidSigningKeyAlias: readEnv("RELEASE_KEYSTORE_ALIAS"),

        androidKeyStorePassword: readEnv("RELEASE_KEYSTORE_PASSWORD"),
        serviceAccountJsonRaw: readEnv("SERVICE_ACCOUNT_JSON", ""),
        serviceAccountJson: readEnv("PLAYSTORE_SERVICE_ACCOUNT_JSON_FILE", ""),

        /**
         * could be timestamp or patch.
         * timestamp will use current DATE and TIME in Seconds.
         * patch will parse number from package.json's version
         */
        buildNumber: "patch"
    },
]