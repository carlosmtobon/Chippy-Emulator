namespace ChipCore
{
    class Sprite
    {
        readonly byte[] _spriteData;

        public Sprite(byte[] spriteData)
        {
            _spriteData = spriteData;
        }

        public byte[] GetSpriteData()
        {
            return _spriteData;
        }
    }
}
