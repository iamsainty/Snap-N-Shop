import express from "express";
import cors from "cors";
import dotenv from "dotenv";
import nodemailer from "nodemailer";

dotenv.config();

const app = express();
app.use(express.json());
app.use(cors());

// Email endpoint
app.post("/send-otp", async (req, res) => {
  const { email, otp } = req.body;

  if (!email || !otp) {
    return res
      .status(400)
      .json({ success: false, message: "Email and OTP are required" });
  }

  try {
    // Transporter configuration
    const transporter = nodemailer.createTransport({
      host: "smtp.gmail.com", // or your SMTP provider
      port: 465,
      secure: true,
      auth: {
        user: process.env.EMAIL_USER,
        pass: process.env.EMAIL_PASS,
      },
    });

    await transporter.sendMail({
      from: `"Snap N Shop" <${process.env.EMAIL_USER}>`,
      to: email,
      subject: "Your Snap N Shop OTP Code",
      html: `<div style='font-family: Arial, sans-serif; font-size: 16px; color: #333;'>
                            <p>Hi there,</p>
                            <p>We received a request to sign in to your <strong>Snap N Shop</strong> account.</p>
                            
                            <p>Your One-Time Password (OTP) is:</p>
                            
                            <span style='display: inline-block; font-size: 36px; font-weight: bold; letter-spacing: 5px; margin: 10px;'>
                                ${otp}
                            </span>
                            
                            <p>This OTP is valid for <strong>10 minutes</strong>. Please do not share it with anyone.</p>
                            
                            <p style='font-size: 14px; color: #777; margin-top: 20px;'>
                                If you did not request this OTP, you can safely ignore this email.
                            </p>
                            
                            <p style='font-size: 14px; color: #777; margin-top: 5px;'>— The Snap N Shop Team</p>
                        </div>`,
    });

    res.status(200).json({ success: true, message: "OTP sent successfully" });
  } catch (error) {
    console.error("Error sending email:", error);
    res.status(500).json({ success: false, message: "Failed to send OTP" });
  }
});

app.get("/", (req, res) => res.send("Snap N Shop Auth API running"));

app.listen(3000, () => console.log("Server running on port 3000"));
