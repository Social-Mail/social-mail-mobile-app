import { readEnv } from "@neurospeech/jex/dist/index.js";

export default [
    {
        id: "in.socialmail.app",
        name: "Social Mail",

        url: "https://mails.socialmail.in",

        certPath: "./cert/ios-distribution.p12",
        certPass: readEnv("APPLE_DISTRIBUTION_CERT_PASS"),
        provisioningProfileFile: readEnv("MOBILE_PROVISIONING_PROFILE", "./cert/ios-app.mobileprovision"),

        appStoreConnect: {
            apiKeyId: "UYZ5D74B6B",
            issuerId: "cb06e4d2-17de-48ae-b58a-b3ce8bec7072",
            privateKey: ""
        },

        passphrase: readEnv("ENC_PASSPHRASE"),

        /**
         * could be timestamp or patch.
         * timestamp will use current DATE and TIME in Seconds.
         * patch will parse number from package.json's version
         */
        buildNumber: "patch"
    },
]