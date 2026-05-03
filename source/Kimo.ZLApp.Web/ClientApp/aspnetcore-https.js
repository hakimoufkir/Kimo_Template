// This script sets up HTTPS for the application using the ASP.NET Core HTTPS certificate.
import fs from "fs";
import path from "path";
import child_process from "child_process";
import crypto from "crypto";

let certFilePath = "";
let keyFilePath = "";

const isNode =
  typeof process !== "undefined" &&
  process.versions != null &&
  process.versions.node != null;

if (!isNode) {
  certFilePath = "";
  keyFilePath = "";
} else {
  const baseFolder =
    process.env.APPDATA && process.env.APPDATA !== ""
      ? `${process.env.APPDATA}/ASP.NET/https`
      : `${process.env.HOME}/.aspnet/https`;

  const certificateArg = process.argv
    .map((arg) => arg.match(/--name=(?<value>.+)/i))
    .filter(Boolean)[0];

  const certificateName =
    certificateArg?.groups?.value || process.env.npm_package_name;

  if (!certificateName) {
    console.error(
      "Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly."
    );
    process.exit(-1);
  }

  certFilePath = path.join(baseFolder, `${certificateName}.pem`);
  keyFilePath = path.join(baseFolder, `${certificateName}.key`);

  if (!fs.existsSync(baseFolder)) {
    fs.mkdirSync(baseFolder, { recursive: true });
  }

  let certExpired = false;

  if (fs.existsSync(certFilePath)) {
    const certContent = fs.readFileSync(certFilePath, "utf8");
    const cert = new crypto.X509Certificate(certContent);
    const now = new Date();
    const certDate = new Date(cert.validTo);

    if (certDate.getTime() < now.getTime()) {
      console.log(`Certificate ${certificateName} is expired. Creating a new one...`);
      certExpired = true;
    }
  }

  if (!fs.existsSync(certFilePath) || !fs.existsSync(keyFilePath) || certExpired) {
    console.log(`Creating a new HTTPS certificate for ${certificateName}...`);

    if (
      child_process.spawnSync(
        "dotnet",
        [
          "dev-certs",
          "https",
          "--export-path",
          certFilePath,
          "--format",
          "Pem",
          "--no-password",
          "--trust"
        ],
        { stdio: "inherit" }
      ).status !== 0
    ) {
      throw new Error("Could not create certificate.");
    }
  }
}

export { certFilePath, keyFilePath };
