namespace AisParser {
    public static class VdmExtensions {

        public static Messages ToMessage(this Vdm vdm) {
            var sixbit = vdm.SixState;
            switch (vdm.MsgId) {
                case 1:
                    return new Message1(sixbit);
                case 2:
                    return new Message2(sixbit);
                case 3:
                    return new Message3(sixbit);
                case 4:
                    return new Message4(sixbit);
                case 5:
                    return new Message5(sixbit);
                case 6:
                    return new Message6(sixbit);
                case 7:
                    return new Message7(sixbit);
                case 8:
                    return new Message8(sixbit);
                case 9:
                    return new Message9(sixbit);
                case 10:
                    return new Message10(sixbit);
                case 11:
                    return new Message11(sixbit);
                case 12:
                    return new Message12(sixbit);
                case 13:
                    return new Message13(sixbit);
                case 14:
                    return new Message14(sixbit);
                case 15:
                    return new Message15(sixbit);
                case 16:
                    return new Message16(sixbit);
                case 17:
                    return new Message17(sixbit);
                case 18:
                    return new Message18(sixbit);
                case 19:
                    return new Message19(sixbit);
                case 20:
                    return new Message20(sixbit);
                case 21:
                    return new Message21(sixbit);
                case 22:
                    return new Message22(sixbit);
                case 23:
                    return new Message23(sixbit);
                case 24:
                    return new Message24(sixbit);
                default:
                    return null;
                    
            }
        }
    }
}