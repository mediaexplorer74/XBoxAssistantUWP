// *********************************************************
// Type: Microsoft.XboxLive.Avatars.Internal.Parsers.LZXDeflate
// Assembly: Microsoft.XboxLive.Avatars, Version=1.2.0.0, Culture=neutral, PublicKeyToken=f156c4aabfd14bbf
// MVID: 684D7B0C-1213-4E8B-93BB-FEE74C9CF841
// *********************************************************Microsoft.XboxLive.Avatars.dll

using System;


namespace Microsoft.XboxLive.Avatars.Internal.Parsers
{
  internal class LZXDeflate
  {
    private const int MIN_MATCH = 2;
    private const int MAX_MATCH = 257;
    private const int NUM_CHARS = 256;
    private const int NUM_PRIMARY_LENGTHS = 7;
    private const int NUM_LENGTHS = 8;
    private const int NUM_SECONDARY_LENGTHS = 249;
    private const int NL_SHIFT = 3;
    private const int NUM_REPEATED_OFFSETS = 3;
    private const int ALIGNED_NUM_ELEMENTS = 8;
    private const int TREE_ENC_REP_MIN = 4;
    private const int TREE_ENC_REP_ZERO_FIRST = 16;
    private const int TREE_ENC_REP_ZERO_SECOND = 32;
    private const int TREE_ENC_REP_SAME_FIRST = 2;
    private const int TREE_ENC_REPZ_FIRST_EXTRA_BITS = 4;
    private const int TREE_ENC_REPZ_SECOND_EXTRA_BITS = 5;
    private const int TREE_ENC_REP_SAME_EXTRA_BITS = 1;
    private const int E8_CFDATA_FRAME_THRESHOLD = 32768;
    private const int MAX_WINDOW_SIZE = 2097152;
    private const int MAX_PARTITION_SIZE = 16777216;
    private const int LZXMARK_END_OF_STREAM = 255;
    private const int DECODER_PREFETCH_PAD_SIZE = 5;
    private const int CHUNK_SIZE = 32768;
    private const int MAX_GROWTH = 6144;
    private const int DS_TABLE_BITS = 8;
    private const int MAX_MAIN_TREE_ELEMENTS = 672;
    private const int MAIN_TREE_TABLE_BITS = 10;
    private const int SECONDARY_LEN_TREE_TABLE_BITS = 8;
    private const int ALIGNED_TABLE_BITS = 7;
    private const int TDECODE_STAGING_BUFFER_SIZE = 32768;
    private const int MAX_COMPRESSED_BLOCK_SIZE = 38922;
    private const int NUM_DECODE_SMALL = 20;
    private int dec_window_size;
    private int dec_window_mask;
    private byte[] dec_mem_window;
    private byte[] dec_extra_bits_table;
    private int[] MP_POS_minus2_table;
    private byte[] dec_main_tree_len;
    private byte[] dec_main_tree_prev_len;
    private byte[] dec_secondary_length_tree_len;
    private byte[] dec_secondary_length_tree_prev_len;
    private uint[] dec_last_matchpos_offset;
    private int dec_bufpos;
    private uint dec_current_file_size;
    private uint dec_instr_pos;
    private uint dec_num_cfdata_frames;
    private LZXDeflate.DecoderState dec_decoder_state;
    private int dec_block_size;
    private int dec_original_block_size;
    private LZXDeflate.BlockType dec_block_type;
    private uint dec_bitbuf;
    private sbyte dec_bitcount;
    private byte dec_num_position_slots;
    private bool dec_first_time_this_group;
    private bool dec_error_condition;
    private byte[] dec_input_buffer;
    private int dec_input_curpos;
    private int dec_end_input_pos;
    private byte[] dec_output_buffer;
    private byte[] dec_aligned_table;
    private byte[] dec_aligned_len;
    private short[] dec_main_tree_table;
    private short[] dec_main_tree_left_right;
    private short[] dec_secondary_length_tree_table;
    private short[] dec_secondary_length_tree_left_right;

    public LZXDeflate(int compression_window_size)
    {
      this.Build_global_tables();
      this.dec_window_size = compression_window_size;
      this.dec_window_mask = this.dec_window_size - 1;
      if ((this.dec_window_size & this.dec_window_mask) != 0 || !this.AllocateMemoryForDecompression())
        return;
      this.dec_main_tree_len = new byte[672];
      this.dec_main_tree_prev_len = new byte[672];
      this.dec_secondary_length_tree_len = new byte[249];
      this.dec_secondary_length_tree_prev_len = new byte[249];
      this.dec_aligned_table = new byte[128];
      this.dec_aligned_len = new byte[8];
      this.dec_last_matchpos_offset = new uint[3];
      this.dec_main_tree_table = new short[1024];
      this.dec_main_tree_left_right = new short[2688];
      this.dec_secondary_length_tree_table = new short[256];
      this.dec_secondary_length_tree_left_right = new short[996];
      this.DecodeNewGroup();
    }

    private int MAIN_TREE_ELEMENTS => 256 + ((int) this.dec_num_position_slots << 3);

    private bool AllocateMemoryForDecompression()
    {
      this.dec_num_position_slots = (byte) 4;
      long num = 4;
      do
      {
        num += 1L << (int) this.dec_extra_bits_table[(int) this.dec_num_position_slots];
        ++this.dec_num_position_slots;
      }
      while (num < (long) this.dec_window_size);
      this.dec_mem_window = new byte[this.dec_window_size + 261];
      return true;
    }

    private static void ZeroArray(ref byte[] array) => Array.Clear((Array) array, 0, array.Length);

    private void DecodeNewGroup()
    {
      LZXDeflate.ZeroArray(ref this.dec_main_tree_len);
      LZXDeflate.ZeroArray(ref this.dec_main_tree_prev_len);
      LZXDeflate.ZeroArray(ref this.dec_secondary_length_tree_len);
      LZXDeflate.ZeroArray(ref this.dec_secondary_length_tree_prev_len);
      this.dec_last_matchpos_offset[0] = 1U;
      this.dec_last_matchpos_offset[1] = 1U;
      this.dec_last_matchpos_offset[2] = 1U;
      this.dec_bufpos = 0;
      this.dec_decoder_state = LZXDeflate.DecoderState.StartNewBlock;
      this.dec_block_size = 0;
      this.dec_original_block_size = 0;
      this.dec_block_type = LZXDeflate.BlockType.Invalid;
      this.dec_first_time_this_group = true;
      this.dec_current_file_size = 0U;
      this.dec_error_condition = false;
      this.dec_instr_pos = 0U;
      this.dec_num_cfdata_frames = 0U;
    }

    private void Build_global_tables()
    {
      this.MP_POS_minus2_table = new int[51];
      int[] mpPosMinus2Table = this.MP_POS_minus2_table;
      if (BitConverter.IsLittleEndian)
        this.dec_extra_bits_table = new byte[52]
        {
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 1,
          (byte) 1,
          (byte) 2,
          (byte) 2,
          (byte) 3,
          (byte) 3,
          (byte) 4,
          (byte) 4,
          (byte) 5,
          (byte) 5,
          (byte) 6,
          (byte) 6,
          (byte) 7,
          (byte) 7,
          (byte) 8,
          (byte) 8,
          (byte) 9,
          (byte) 9,
          (byte) 10,
          (byte) 10,
          (byte) 11,
          (byte) 11,
          (byte) 12,
          (byte) 12,
          (byte) 13,
          (byte) 13,
          (byte) 14,
          (byte) 14,
          (byte) 15,
          (byte) 15,
          (byte) 16,
          (byte) 16,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17
        };
      else
        this.dec_extra_bits_table = new byte[52]
        {
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 0,
          (byte) 2,
          (byte) 2,
          (byte) 1,
          (byte) 1,
          (byte) 4,
          (byte) 4,
          (byte) 3,
          (byte) 3,
          (byte) 6,
          (byte) 6,
          (byte) 5,
          (byte) 5,
          (byte) 8,
          (byte) 8,
          (byte) 7,
          (byte) 7,
          (byte) 10,
          (byte) 10,
          (byte) 9,
          (byte) 9,
          (byte) 12,
          (byte) 12,
          (byte) 11,
          (byte) 11,
          (byte) 14,
          (byte) 14,
          (byte) 13,
          (byte) 13,
          (byte) 16,
          (byte) 16,
          (byte) 15,
          (byte) 15,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17,
          (byte) 17
        };
      mpPosMinus2Table[0] = -2;
      mpPosMinus2Table[1] = -1;
      mpPosMinus2Table[2] = 0;
      mpPosMinus2Table[3] = 1;
      mpPosMinus2Table[4] = 2;
      mpPosMinus2Table[5] = 4;
      mpPosMinus2Table[6] = 6;
      mpPosMinus2Table[7] = 10;
      mpPosMinus2Table[8] = 14;
      mpPosMinus2Table[9] = 22;
      mpPosMinus2Table[10] = 30;
      mpPosMinus2Table[11] = 46;
      mpPosMinus2Table[12] = 62;
      mpPosMinus2Table[13] = 94;
      mpPosMinus2Table[14] = 126;
      mpPosMinus2Table[15] = 190;
      mpPosMinus2Table[16] = 254;
      mpPosMinus2Table[17] = 382;
      mpPosMinus2Table[18] = 510;
      mpPosMinus2Table[19] = 766;
      mpPosMinus2Table[20] = 1022;
      mpPosMinus2Table[21] = 1534;
      mpPosMinus2Table[22] = 2046;
      mpPosMinus2Table[23] = 3070;
      mpPosMinus2Table[24] = 4094;
      mpPosMinus2Table[25] = 6142;
      mpPosMinus2Table[26] = 8190;
      mpPosMinus2Table[27] = 12286;
      mpPosMinus2Table[28] = 16382;
      mpPosMinus2Table[29] = 24574;
      mpPosMinus2Table[30] = 32766;
      mpPosMinus2Table[31] = 49150;
      mpPosMinus2Table[32] = 65534;
      mpPosMinus2Table[33] = 98302;
      mpPosMinus2Table[34] = 131070;
      mpPosMinus2Table[35] = 196606;
      mpPosMinus2Table[36] = 262142;
      mpPosMinus2Table[37] = 393214;
      mpPosMinus2Table[38] = 524286;
      mpPosMinus2Table[39] = 655358;
      mpPosMinus2Table[40] = 786430;
      mpPosMinus2Table[41] = 917502;
      mpPosMinus2Table[42] = 1048574;
      mpPosMinus2Table[43] = 1179646;
      mpPosMinus2Table[44] = 1310718;
      mpPosMinus2Table[45] = 1441790;
      mpPosMinus2Table[46] = 1572862;
      mpPosMinus2Table[47] = 1703934;
      mpPosMinus2Table[48] = 1835006;
      mpPosMinus2Table[49] = 1966078;
      mpPosMinus2Table[50] = 2097150;
    }

    public bool Reset()
    {
      this.DecodeNewGroup();
      return true;
    }

    public int Decompress(ref byte[] src, ref byte[] tg)
    {
      this.dec_input_buffer = src;
      this.dec_input_curpos = 0;
      this.dec_end_input_pos = src.Length;
      this.dec_output_buffer = tg;
      this.initialise_decoder_bitbuf();
      int num = this.decode_data(tg.Length);
      ++this.dec_num_cfdata_frames;
      return num;
    }

    private int decode_data(int bytes_to_decode)
    {
      int num = 0;
      while (bytes_to_decode > 0)
      {
        if (this.dec_decoder_state == LZXDeflate.DecoderState.StartNewBlock)
        {
          if (this.dec_first_time_this_group)
          {
            this.dec_first_time_this_group = false;
            this.dec_current_file_size = this.getbits(1) == 0U ? 0U : this.getbits(16) << 16 | this.getbits(16);
          }
          if (this.dec_block_type == LZXDeflate.BlockType.Uncompressed)
          {
            if ((this.dec_original_block_size & 1) > 0 && this.dec_input_curpos < this.dec_end_input_pos)
              ++this.dec_input_curpos;
            this.dec_block_type = LZXDeflate.BlockType.Invalid;
            this.initialise_decoder_bitbuf();
          }
          this.dec_block_type = (LZXDeflate.BlockType) this.getbits(3);
          this.dec_block_size = ((int) this.getbits(8) << 16) + ((int) this.getbits(8) << 8) + (int) this.getbits(8);
          this.dec_original_block_size = this.dec_block_size;
          if (this.dec_block_type == LZXDeflate.BlockType.Aligned)
            this.read_aligned_offset_tree();
          if (this.dec_block_type == LZXDeflate.BlockType.Verbatim || this.dec_block_type == LZXDeflate.BlockType.Aligned)
          {
            Buffer.BlockCopy((Array) this.dec_main_tree_len, 0, (Array) this.dec_main_tree_prev_len, 0, this.MAIN_TREE_ELEMENTS);
            Buffer.BlockCopy((Array) this.dec_secondary_length_tree_len, 0, (Array) this.dec_secondary_length_tree_prev_len, 0, 249);
            this.read_main_and_secondary_trees();
          }
          else if (this.dec_block_type != LZXDeflate.BlockType.Uncompressed || !this.handle_beginning_of_uncompressed_block())
            return -1;
          this.dec_decoder_state = LZXDeflate.DecoderState.DecodingData;
        }
        while (this.dec_block_size > 0 && bytes_to_decode > 0)
        {
          uint amount_to_decode = (uint) Math.Min(this.dec_block_size, bytes_to_decode);
          if (amount_to_decode == 0U || this.decode_block(this.dec_block_type, this.dec_bufpos, amount_to_decode) != 0)
            return -1;
          this.dec_block_size -= (int) amount_to_decode;
          bytes_to_decode -= (int) amount_to_decode;
          num += (int) amount_to_decode;
        }
        if (this.dec_block_size == 0)
          this.dec_decoder_state = LZXDeflate.DecoderState.StartNewBlock;
        if (bytes_to_decode == 0)
          this.initialise_decoder_bitbuf();
      }
      bool flag = this.dec_current_file_size != 0U && this.dec_num_cfdata_frames < 32768U;
      if (this.dec_bufpos > 0)
        Buffer.BlockCopy((Array) this.dec_mem_window, this.dec_bufpos - num, (Array) this.dec_output_buffer, 0, num);
      else
        Buffer.BlockCopy((Array) this.dec_mem_window, this.dec_window_size - num, (Array) this.dec_output_buffer, 0, num);
      if (flag)
        this.decoder_translate_e8(ref this.dec_output_buffer, (long) num);
      return num;
    }

    private void fillbuf(int n)
    {
      this.dec_bitbuf <<= n;
      this.dec_bitcount -= (sbyte) n;
      if (this.dec_bitcount > (sbyte) 0)
        return;
      if (this.dec_input_curpos >= this.dec_end_input_pos)
      {
        this.dec_error_condition = true;
      }
      else
      {
        if (this.dec_input_curpos + 1 >= this.dec_end_input_pos)
          return;
        this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos++] | (uint) this.dec_input_buffer[this.dec_input_curpos++] << 8) << (int) -this.dec_bitcount;
        this.dec_bitcount += (sbyte) 16;
        if (this.dec_bitcount > (sbyte) 0)
          return;
        if (this.dec_input_curpos >= this.dec_end_input_pos)
        {
          this.dec_error_condition = true;
        }
        else
        {
          this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos++] | (uint) this.dec_input_buffer[this.dec_input_curpos++] << 8) << (int) -this.dec_bitcount;
          this.dec_bitcount += (sbyte) 16;
        }
      }
    }

    private uint getbits(int n)
    {
      uint num = this.dec_bitbuf >> 32 - n;
      this.fillbuf(n);
      return num;
    }

    private static bool make_table_8bit(ref byte[] bitlen, ref byte[] table)
    {
      ushort[] numArray1 = new ushort[17];
      ushort[] numArray2 = new ushort[17];
      ushort[] numArray3 = new ushort[18];
      for (ushort index = 1; index <= (ushort) 16; ++index)
        numArray1[(int) index] = (ushort) 0;
      for (ushort index = 0; index < (ushort) 8; ++index)
        ++numArray1[(int) bitlen[(int) index]];
      numArray3[1] = (ushort) 0;
      for (ushort index = 1; index <= (ushort) 16; ++index)
        numArray3[(int) index + 1] = (ushort) ((uint) numArray3[(int) index] + ((uint) numArray1[(int) index] << 16 - (int) index));
      if (numArray3[17] != (ushort) 0)
        return false;
      ushort index1;
      for (index1 = (ushort) 1; index1 <= (ushort) 7; ++index1)
      {
        numArray3[(int) index1] >>= 9;
        numArray2[(int) index1] = (ushort) (1 << 7 - (int) index1);
      }
      for (; index1 <= (ushort) 16; ++index1)
        numArray2[(int) index1] = (ushort) (1 << 16 - (int) index1);
      LZXDeflate.ZeroArray(ref table);
      for (byte index2 = 0; index2 < (byte) 8; ++index2)
      {
        byte index3;
        if ((index3 = bitlen[(int) index2]) != (byte) 0)
        {
          ushort num = (ushort) ((uint) numArray3[(int) index3] + (uint) numArray2[(int) index3]);
          if (num > (ushort) 128)
            return false;
          for (ushort index4 = numArray3[(int) index3]; (int) index4 < (int) num; ++index4)
            table[(int) index4] = index2;
          numArray3[(int) index3] = num;
        }
      }
      return true;
    }

    private bool read_aligned_offset_tree()
    {
      for (int index = 0; index < 8; ++index)
        this.dec_aligned_len[index] = (byte) this.getbits(3);
      return !this.dec_error_condition && LZXDeflate.make_table_8bit(ref this.dec_aligned_len, ref this.dec_aligned_table);
    }

    private static bool make_table(
      int nchar,
      ref byte[] bitlen,
      byte tablebits,
      ref short[] table,
      ref short[] leftright)
    {
      uint[] numArray1 = new uint[17];
      uint[] numArray2 = new uint[17];
      uint[] numArray3 = new uint[18];
      for (uint index = 1; index <= 16U; ++index)
        numArray1[(IntPtr) index] = 0U;
      for (uint index = 0; index < (uint) nchar; ++index)
        ++numArray1[(int) bitlen[(IntPtr) index]];
      numArray3[1] = 0U;
      for (uint index = 1; index <= 16U; ++index)
        numArray3[(IntPtr) (index + 1U)] = numArray3[(IntPtr) index] + (numArray1[(IntPtr) index] << (int) (byte) (16U - index));
      if (numArray3[17] != 65536U)
      {
        if (numArray3[17] != 0U)
          return false;
        for (int index = 0; (long) index < (long) (1U << (int) tablebits); ++index)
          table[index] = (short) 0;
        return true;
      }
      byte num1 = (byte) (16U - (uint) tablebits);
      uint index1;
      for (index1 = 1U; index1 <= (uint) tablebits; ++index1)
      {
        numArray3[(IntPtr) index1] >>= (int) num1;
        numArray2[(IntPtr) index1] = 1U << (int) (byte) ((uint) tablebits - index1);
      }
      for (; index1 <= 16U; ++index1)
        numArray2[(IntPtr) index1] = 1U << (int) (byte) (16U - index1);
      uint num2 = numArray3[(int) tablebits + 1] >> (int) num1;
      if (num2 != 65536U)
      {
        for (uint index2 = 0; (long) index2 < (long) (1 << (int) tablebits) - (long) num2; ++index2)
          table[(IntPtr) (num2 + index2)] = (short) 0;
      }
      int num3 = nchar;
      for (int index3 = 0; index3 < nchar; ++index3)
      {
        byte index4;
        if ((index4 = bitlen[index3]) != (byte) 0)
        {
          uint num4 = numArray3[(int) index4] + numArray2[(int) index4];
          if ((int) index4 <= (int) tablebits)
          {
            if (num4 > 1U << (int) tablebits)
              return false;
            for (uint index5 = numArray3[(int) index4]; index5 < num4; ++index5)
              table[(IntPtr) index5] = (short) index3;
            numArray3[(int) index4] = num4;
          }
          else
          {
            uint num5 = numArray3[(int) index4];
            numArray3[(int) index4] = num4;
            short[] numArray4 = table;
            int index6 = (int) (num5 >> (int) num1);
            byte num6 = (byte) ((uint) index4 - (uint) tablebits);
            uint num7 = num5 << (int) tablebits;
            do
            {
              if (numArray4[index6] == (short) 0)
              {
                leftright[num3 * 2] = leftright[num3 * 2 + 1] = (short) 0;
                numArray4[index6] = (short) -num3;
                ++num3;
              }
              if ((short) num7 < (short) 0)
              {
                index6 = (int) -numArray4[index6] * 2 + 1;
                numArray4 = leftright;
              }
              else
              {
                index6 = (int) -numArray4[index6] * 2;
                numArray4 = leftright;
              }
              num7 <<= 1;
              --num6;
            }
            while (num6 > (byte) 0);
            numArray4[index6] = (short) index3;
          }
        }
      }
      return true;
    }

    private void DECODE_FILLBUF(int n)
    {
      this.dec_bitbuf <<= n;
      this.dec_bitcount -= (sbyte) n;
      if (this.dec_bitcount > (sbyte) 0)
        return;
      if (this.dec_input_curpos >= this.dec_end_input_pos)
      {
        this.dec_error_condition = true;
      }
      else
      {
        this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos] | (uint) this.dec_input_buffer[this.dec_input_curpos + 1] << 8) << (int) -this.dec_bitcount;
        this.dec_input_curpos += 2;
        this.dec_bitcount += (sbyte) 16;
        if (this.dec_bitcount > (sbyte) 0)
          return;
        if (this.dec_input_curpos >= this.dec_end_input_pos)
        {
          this.dec_error_condition = true;
        }
        else
        {
          this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos] | (uint) this.dec_input_buffer[this.dec_input_curpos + 1] << 8) << (int) -this.dec_bitcount;
          this.dec_input_curpos += 2;
          this.dec_bitcount += (sbyte) 16;
        }
      }
    }

    private bool DECODE_SMALL(
      ref int small_table_idx,
      ref short[] small_table,
      ref uint mask,
      ref int leftright_idx,
      ref short[] leftright_s,
      ref byte[] small_bitlen,
      ref short item)
    {
      small_table_idx = (int) (this.dec_bitbuf >> 24);
      if (small_table_idx >= small_table.Length)
      {
        this.dec_error_condition = true;
        return false;
      }
      item = small_table[small_table_idx];
      if (item < (short) 0)
      {
        int num = 23;
        mask = (uint) (1 << num);
        do
        {
          item = -item;
          leftright_idx = ((int) this.dec_bitbuf & (int) mask) == 0 ? 2 * (int) item : 2 * (int) item + 1;
          if (leftright_idx >= leftright_s.Length)
          {
            this.dec_error_condition = true;
            return false;
          }
          item = leftright_s[leftright_idx];
          mask >>= 1;
        }
        while (item < (short) 0);
      }
      if ((int) item >= small_bitlen.Length)
      {
        this.dec_error_condition = true;
        return false;
      }
      this.DECODE_FILLBUF((int) small_bitlen[(int) item]);
      return true;
    }

    private void DECODE_GETBITS(ref int dest, int n)
    {
      dest = (int) (byte) (this.dec_bitbuf >> 32 - n);
      this.DECODE_FILLBUF(n);
    }

    private bool ReadRepTree(
      int num_elements,
      ref byte[] dec_tree_prev_len,
      int lastlen,
      ref byte[] dec_tree_len,
      int len)
    {
      uint mask = 0;
      int dest = 0;
      byte[] numArray1 = new byte[24];
      short[] numArray2 = new short[256];
      short[] numArray3 = new short[94];
      short num1 = 0;
      int small_table_idx = 0;
      int leftright_idx = 0;
      for (int index = 0; index < 20; ++index)
        numArray1[index] = (byte) this.getbits(4);
      if (this.dec_error_condition)
        return false;
      LZXDeflate.make_table(20, ref numArray1, (byte) 8, ref numArray2, ref numArray3);
      for (int index = 0; index < num_elements; ++index)
      {
        this.DECODE_SMALL(ref small_table_idx, ref numArray2, ref mask, ref leftright_idx, ref numArray3, ref numArray1, ref num1);
        if (!this.dec_error_condition)
        {
          switch (num1)
          {
            case 17:
              this.DECODE_GETBITS(ref dest, 4);
              dest += 4;
              if (index + dest >= num_elements)
                dest = num_elements - index;
              while (dest-- > 0)
              {
                dec_tree_len[len + index] = (byte) 0;
                ++index;
              }
              --index;
              break;
            case 18:
              this.DECODE_GETBITS(ref dest, 5);
              dest += 20;
              if (index + dest >= num_elements)
                dest = num_elements - index;
              while (dest-- > 0)
              {
                dec_tree_len[len + index] = (byte) 0;
                ++index;
              }
              --index;
              break;
            case 19:
              this.DECODE_GETBITS(ref dest, 1);
              dest += 4;
              if (index + dest >= num_elements)
                dest = num_elements - index;
              this.DECODE_SMALL(ref small_table_idx, ref numArray2, ref mask, ref leftright_idx, ref numArray3, ref numArray1, ref num1);
              int num2 = (int) dec_tree_prev_len[lastlen + index] - (int) num1 + 17;
              if (num2 >= 17)
                num2 -= 17;
              byte num3 = (byte) num2;
              while (dest-- > 0)
              {
                dec_tree_len[len + index] = num3;
                ++index;
              }
              --index;
              break;
            default:
              int num4 = (int) dec_tree_prev_len[lastlen + index] - (int) num1 + 17;
              if (num4 >= 17)
                num4 -= 17;
              byte num5 = (byte) num4;
              dec_tree_len[len + index] = num5;
              break;
          }
        }
        else
          break;
      }
      return !this.dec_error_condition;
    }

    private bool read_main_and_secondary_trees()
    {
      return this.ReadRepTree(256, ref this.dec_main_tree_prev_len, 0, ref this.dec_main_tree_len, 0) && this.ReadRepTree((int) this.dec_num_position_slots * 8, ref this.dec_main_tree_prev_len, 256, ref this.dec_main_tree_len, 256) && LZXDeflate.make_table(this.MAIN_TREE_ELEMENTS, ref this.dec_main_tree_len, (byte) 10, ref this.dec_main_tree_table, ref this.dec_main_tree_left_right) && this.ReadRepTree(249, ref this.dec_secondary_length_tree_prev_len, 0, ref this.dec_secondary_length_tree_len, 0) && LZXDeflate.make_table(249, ref this.dec_secondary_length_tree_len, (byte) 8, ref this.dec_secondary_length_tree_table, ref this.dec_secondary_length_tree_left_right);
    }

    private void decoder_translate_e8(ref byte[] mem, long bytes)
    {
      byte[] numArray = new byte[6];
      int num1 = 0;
      if (bytes <= 6L)
      {
        this.dec_instr_pos += (uint) bytes;
      }
      else
      {
        byte[] dst = mem;
        Buffer.BlockCopy((Array) mem, (int) (bytes - 6L), (Array) numArray, 0, 6);
        mem[bytes - 6L] = (byte) 232;
        mem[bytes - 5L] = (byte) 232;
        mem[bytes - 4L] = (byte) 232;
        mem[bytes - 3L] = (byte) 232;
        mem[bytes - 2L] = (byte) 232;
        mem[bytes - 1L] = (byte) 232;
        uint num2 = (uint) ((ulong) this.dec_instr_pos + (ulong) bytes - 10UL);
        while (true)
        {
          uint num3 = 0;
          int num4 = 0;
          while (mem[num4++] != (byte) 232)
            ++num3;
          int startIndex = num4;
          this.dec_instr_pos += num3;
          if (this.dec_instr_pos < num2)
          {
            uint uint32 = BitConverter.ToUInt32(mem, startIndex);
            if (uint32 < this.dec_current_file_size)
            {
              byte[] bytes1 = BitConverter.GetBytes(uint32 - this.dec_instr_pos);
              mem[startIndex] = bytes1[0];
              mem[startIndex + 1] = bytes1[1];
              mem[startIndex + 2] = bytes1[2];
              mem[startIndex + 3] = bytes1[3];
            }
            else if ((ulong) -uint32 <= (ulong) this.dec_instr_pos)
            {
              byte[] bytes2 = BitConverter.GetBytes(uint32 + this.dec_current_file_size);
              mem[startIndex] = bytes2[0];
              mem[startIndex + 1] = bytes2[1];
              mem[startIndex + 2] = bytes2[2];
              mem[startIndex + 3] = bytes2[3];
            }
            num1 = startIndex + 4;
            this.dec_instr_pos += 5U;
          }
          else
            break;
        }
        this.dec_instr_pos = num2 + 10U;
        Buffer.BlockCopy((Array) numArray, 0, (Array) dst, (int) (bytes - 6L), 6);
      }
    }

    private int decode_block(LZXDeflate.BlockType block_type, int bufpos, uint amount_to_decode)
    {
      int num;
      switch (block_type)
      {
        case LZXDeflate.BlockType.Verbatim:
          num = this.decode_verbatim_block(bufpos, (int) amount_to_decode);
          break;
        case LZXDeflate.BlockType.Aligned:
          num = this.decode_aligned_offset_block(bufpos, (int) amount_to_decode);
          break;
        case LZXDeflate.BlockType.Uncompressed:
          num = this.decode_uncompressed_block(bufpos, (int) amount_to_decode);
          break;
        default:
          num = -1;
          break;
      }
      return num;
    }

    private int decode_aligned_offset_block(int bufpos, int amount_to_decode)
    {
      if (bufpos < 257)
      {
        int amount_to_decode1 = Math.Min(257 - bufpos, amount_to_decode);
        int num = this.special_decode_aligned_block(bufpos, amount_to_decode1);
        amount_to_decode -= num - bufpos;
        this.dec_bufpos = bufpos = num;
        if (amount_to_decode <= 0)
          return amount_to_decode;
      }
      return this.fast_decode_aligned_offset_block(bufpos, amount_to_decode);
    }

    private int special_decode_aligned_block(int bufpos, int amount_to_decode)
    {
      int num1 = bufpos + amount_to_decode;
      while (bufpos < num1)
      {
        int num2;
        if ((num2 = this.DecodeMainTree() - 256) < 0)
        {
          this.dec_mem_window[bufpos] = (byte) num2;
          this.dec_mem_window[this.dec_window_size + bufpos] = (byte) num2;
          ++bufpos;
        }
        else
        {
          int matchlen;
          if ((matchlen = num2 & 7) == 7)
            this.DecodeLenTreeNoEofCheck(ref matchlen);
          sbyte index = (sbyte) (num2 >> 3);
          uint num3;
          if (index > (sbyte) 2)
          {
            if (this.dec_extra_bits_table[(int) index] >= (byte) 3)
            {
              uint bitsNoEofCheck = (int) this.dec_extra_bits_table[(int) index] - 3 <= 0 ? 0U : this.GetBitsNoEofCheck((int) this.dec_extra_bits_table[(int) index] - 3);
              num3 = (uint) (this.MP_POS_minus2_table[(int) index] + ((int) bitsNoEofCheck << 3)) + this.DecodeAlignedNoEofCheck();
            }
            else
              num3 = this.dec_extra_bits_table[(int) index] <= (byte) 0 ? 1U : this.GetBitsNoEofCheck((int) this.dec_extra_bits_table[(int) index]) + (uint) this.MP_POS_minus2_table[(int) index];
            this.dec_last_matchpos_offset[2] = this.dec_last_matchpos_offset[1];
            this.dec_last_matchpos_offset[1] = this.dec_last_matchpos_offset[0];
            this.dec_last_matchpos_offset[0] = num3;
          }
          else
          {
            num3 = this.dec_last_matchpos_offset[(int) index];
            this.dec_last_matchpos_offset[(int) index] = this.dec_last_matchpos_offset[0];
            this.dec_last_matchpos_offset[0] = num3;
          }
          int num4 = matchlen + 2;
          do
          {
            uint num5 = (uint) this.dec_mem_window[(long) bufpos - (long) num3 & (long) this.dec_window_mask];
            this.dec_mem_window[bufpos] = (byte) num5;
            if (bufpos < 257)
              this.dec_mem_window[this.dec_window_size + bufpos] = (byte) num5;
            ++bufpos;
          }
          while (--num4 > 0);
        }
      }
      return bufpos;
    }

    private int fast_decode_aligned_offset_block(int bufpos, int amount_to_decode)
    {
      int num1 = bufpos + amount_to_decode;
      while (bufpos < num1)
      {
        int num2;
        if ((num2 = this.DecodeMainTree() - 256) < 0)
        {
          this.dec_mem_window[bufpos++] = (byte) num2;
        }
        else
        {
          int matchlen;
          if ((matchlen = num2 & 7) == 7)
            this.DecodeLenTreeNoEofCheck(ref matchlen);
          sbyte index = (sbyte) (num2 >> 3);
          uint num3;
          if (index > (sbyte) 2)
          {
            if (this.dec_extra_bits_table[(int) index] >= (byte) 3)
            {
              uint bitsNoEofCheck = (int) this.dec_extra_bits_table[(int) index] - 3 <= 0 ? 0U : this.GetBitsNoEofCheck((int) this.dec_extra_bits_table[(int) index] - 3);
              num3 = (uint) (this.MP_POS_minus2_table[(int) index] + ((int) bitsNoEofCheck << 3)) + this.DecodeAlignedNoEofCheck();
            }
            else
              num3 = this.dec_extra_bits_table[(int) index] <= (byte) 0 ? (uint) this.MP_POS_minus2_table[(int) index] : this.GetBitsNoEofCheck((int) this.dec_extra_bits_table[(int) index]) + (uint) this.MP_POS_minus2_table[(int) index];
            this.dec_last_matchpos_offset[2] = this.dec_last_matchpos_offset[1];
            this.dec_last_matchpos_offset[1] = this.dec_last_matchpos_offset[0];
            this.dec_last_matchpos_offset[0] = num3;
          }
          else
          {
            num3 = this.dec_last_matchpos_offset[(int) index];
            this.dec_last_matchpos_offset[(int) index] = this.dec_last_matchpos_offset[0];
            this.dec_last_matchpos_offset[0] = num3;
          }
          int num4 = matchlen + 2;
          uint num5 = (uint) (bufpos - (int) num3 & this.dec_window_mask);
          do
          {
            this.dec_mem_window[bufpos++] = this.dec_mem_window[(IntPtr) num5++];
          }
          while (--num4 > 0);
        }
      }
      int num6 = bufpos - num1;
      bufpos &= this.dec_window_mask;
      this.dec_bufpos = bufpos;
      return num6;
    }

    private int decode_verbatim_block(int bufpos, int amount_to_decode)
    {
      if (bufpos < 257)
      {
        int amount_to_decode1 = Math.Min(257 - bufpos, amount_to_decode);
        int num = this.special_decode_verbatim_block(bufpos, amount_to_decode1);
        amount_to_decode -= num - bufpos;
        this.dec_bufpos = bufpos = num;
        if (amount_to_decode <= 0)
          return amount_to_decode;
      }
      return this.fast_decode_verbatim_block(bufpos, amount_to_decode);
    }

    private int special_decode_verbatim_block(int bufpos, int amount_to_decode)
    {
      int num1 = bufpos + amount_to_decode;
      while (bufpos < num1)
      {
        int num2;
        if ((num2 = this.DecodeMainTree() - 256) < 0)
        {
          this.dec_mem_window[bufpos] = (byte) num2;
          this.dec_mem_window[this.dec_window_size + bufpos] = (byte) num2;
          ++bufpos;
        }
        else
        {
          int matchlen;
          if ((matchlen = num2 & 7) == 7)
            this.DecodeLenTreeNoEofCheck(ref matchlen);
          sbyte index = (sbyte) (num2 >> 3);
          uint num3;
          if (index > (sbyte) 2)
          {
            num3 = index <= (sbyte) 3 ? 1U : this.GetBits17NoEofCheck((int) this.dec_extra_bits_table[(int) index]) + (uint) this.MP_POS_minus2_table[(int) index];
            this.dec_last_matchpos_offset[2] = this.dec_last_matchpos_offset[1];
            this.dec_last_matchpos_offset[1] = this.dec_last_matchpos_offset[0];
            this.dec_last_matchpos_offset[0] = num3;
          }
          else
          {
            num3 = this.dec_last_matchpos_offset[(int) index];
            if (index > (sbyte) 0)
            {
              this.dec_last_matchpos_offset[(int) index] = this.dec_last_matchpos_offset[0];
              this.dec_last_matchpos_offset[0] = num3;
            }
          }
          int num4 = matchlen + 2;
          do
          {
            this.dec_mem_window[bufpos] = this.dec_mem_window[(long) bufpos - (long) num3 & (long) this.dec_window_mask];
            if (bufpos < 257)
              this.dec_mem_window[this.dec_window_size + bufpos] = this.dec_mem_window[bufpos];
            ++bufpos;
          }
          while (--num4 > 0);
        }
      }
      return bufpos;
    }

    private int fast_decode_verbatim_block(int bufpos, int amount_to_decode)
    {
      int num1 = bufpos + amount_to_decode;
      while (bufpos < num1)
      {
        int num2;
        if ((num2 = this.DecodeMainTree() - 256) < 0)
        {
          this.dec_mem_window[bufpos++] = (byte) num2;
        }
        else
        {
          int matchlen;
          if ((matchlen = num2 & 7) == 7)
            this.DecodeLenTreeNoEofCheck(ref matchlen);
          sbyte index = (sbyte) (num2 >> 3);
          uint num3;
          if (index > (sbyte) 2)
          {
            num3 = index <= (sbyte) 3 ? (uint) this.MP_POS_minus2_table[3] : this.GetBits17NoEofCheck((int) this.dec_extra_bits_table[(int) index]) + (uint) this.MP_POS_minus2_table[(int) index];
            this.dec_last_matchpos_offset[2] = this.dec_last_matchpos_offset[1];
            this.dec_last_matchpos_offset[1] = this.dec_last_matchpos_offset[0];
            this.dec_last_matchpos_offset[0] = num3;
          }
          else
          {
            num3 = this.dec_last_matchpos_offset[(int) index];
            if (index > (sbyte) 0)
            {
              this.dec_last_matchpos_offset[(int) index] = this.dec_last_matchpos_offset[0];
              this.dec_last_matchpos_offset[0] = num3;
            }
          }
          int num4 = matchlen + 2;
          uint num5 = (uint) (bufpos - (int) num3 & this.dec_window_mask);
          do
          {
            this.dec_mem_window[bufpos++] = this.dec_mem_window[(IntPtr) num5++];
          }
          while (--num4 > 0);
        }
      }
      int num6 = bufpos - num1;
      bufpos &= this.dec_window_mask;
      this.dec_bufpos = bufpos;
      return num6;
    }

    private int decode_uncompressed_block(int bufpos, int amount_to_decode)
    {
      int val2 = bufpos + amount_to_decode;
      int num1 = Math.Min(amount_to_decode, this.dec_end_input_pos - this.dec_input_curpos);
      int num2 = bufpos;
      int num3 = bufpos + num1;
      while (bufpos < num3)
        this.dec_mem_window[bufpos++] = this.dec_input_buffer[this.dec_input_curpos++];
      if (num3 != val2)
        return -1;
      int num4 = Math.Min(257, val2) - num2;
      int num5 = num2 + this.dec_window_size;
      int num6 = num2;
      int num7 = num5 + num4;
      while (num5 < num7)
        this.dec_mem_window[num5++] = this.dec_input_buffer[num6++];
      int num8 = bufpos - val2;
      bufpos &= this.dec_window_mask;
      this.dec_bufpos = bufpos;
      return num8;
    }

    private bool handle_beginning_of_uncompressed_block()
    {
      this.dec_input_curpos -= 2;
      if (this.dec_input_curpos + 4 >= this.dec_end_input_pos)
        return false;
      for (int index = 0; index < 3; ++index)
      {
        byte num1 = this.dec_input_buffer[this.dec_input_curpos++];
        byte num2 = this.dec_input_buffer[this.dec_input_curpos++];
        byte num3 = this.dec_input_buffer[this.dec_input_curpos++];
        byte num4 = this.dec_input_buffer[this.dec_input_curpos++];
        this.dec_last_matchpos_offset[index] = (uint) ((int) num1 | (int) num2 << 8 | (int) num3 << 16 | (int) num4 << 24);
      }
      return true;
    }

    private uint DecodeAlignedNoEofCheck()
    {
      uint index = (uint) this.dec_aligned_table[(IntPtr) (this.dec_bitbuf >> 25)];
      this.FillBufNoEofCheck((int) this.dec_aligned_len[(IntPtr) index]);
      return index;
    }

    private void initialise_decoder_bitbuf()
    {
      if (this.dec_block_type == LZXDeflate.BlockType.Uncompressed || this.dec_input_curpos + 4 > this.dec_end_input_pos)
        return;
      this.dec_bitbuf = (uint) ((int) this.dec_input_buffer[this.dec_input_curpos++] | (int) this.dec_input_buffer[this.dec_input_curpos++] << 8 | ((int) this.dec_input_buffer[this.dec_input_curpos++] | (int) this.dec_input_buffer[this.dec_input_curpos++] << 8) << 16);
      this.dec_bitcount = (sbyte) 16;
    }

    private void FillBufFillCheck(int N)
    {
      if (this.dec_input_curpos > this.dec_end_input_pos)
        return;
      this.dec_bitbuf <<= N;
      this.dec_bitcount -= (sbyte) N;
      if (this.dec_bitcount > (sbyte) 0 || this.dec_input_curpos + 1 >= this.dec_end_input_pos)
        return;
      this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos++] | (uint) this.dec_input_buffer[this.dec_input_curpos++] << 8) << (int) -this.dec_bitcount;
      this.dec_bitcount += (sbyte) 16;
    }

    private void FillBufNoEofCheck(int N)
    {
      this.dec_bitbuf <<= N;
      this.dec_bitcount -= (sbyte) N;
      if (this.dec_bitcount > (sbyte) 0 || this.dec_input_curpos + 1 >= this.dec_end_input_pos)
        return;
      this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos++] | (uint) this.dec_input_buffer[this.dec_input_curpos++] << 8) << (int) -this.dec_bitcount;
      this.dec_bitcount += (sbyte) 16;
    }

    private void FillBuf17NoEofCheck(int N)
    {
      this.dec_bitbuf <<= N;
      this.dec_bitcount -= (sbyte) N;
      if (this.dec_bitcount > (sbyte) 0 || this.dec_input_curpos + 1 >= this.dec_end_input_pos)
        return;
      this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos++] | (uint) this.dec_input_buffer[this.dec_input_curpos++] << 8) << (int) -this.dec_bitcount;
      this.dec_bitcount += (sbyte) 16;
      if (this.dec_bitcount > (sbyte) 0)
        return;
      this.dec_bitbuf |= ((uint) this.dec_input_buffer[this.dec_input_curpos++] | (uint) this.dec_input_buffer[this.dec_input_curpos++] << 8) << (int) -this.dec_bitcount;
      this.dec_bitcount += (sbyte) 16;
    }

    private int DecodeMainTree()
    {
      int index = (int) this.dec_main_tree_table[(IntPtr) (this.dec_bitbuf >> 22)];
      if (index < 0)
      {
        uint num1 = (uint) (1 << 21);
        do
        {
          int num2 = -index;
          index = (this.dec_bitbuf & num1) <= 0U ? (int) this.dec_main_tree_left_right[num2 * 2] : (int) this.dec_main_tree_left_right[num2 * 2 + 1];
          num1 >>= 1;
        }
        while (index < 0);
      }
      this.FillBufFillCheck((int) this.dec_main_tree_len[index]);
      return index;
    }

    private void DecodeLenTreeNoEofCheck(ref int matchlen)
    {
      matchlen = (int) this.dec_secondary_length_tree_table[(IntPtr) (this.dec_bitbuf >> 24)];
      if (matchlen < 0)
      {
        uint num = (uint) (1 << 23);
        do
        {
          matchlen = -matchlen;
          matchlen = (this.dec_bitbuf & num) <= 0U ? (int) this.dec_secondary_length_tree_left_right[matchlen * 2] : (int) this.dec_secondary_length_tree_left_right[matchlen * 2 + 1];
          num >>= 1;
        }
        while (matchlen < 0);
      }
      this.FillBufNoEofCheck((int) this.dec_secondary_length_tree_len[matchlen]);
      matchlen += 7;
    }

    private uint GetBitsNoEofCheck(int N)
    {
      uint bitsNoEofCheck = this.dec_bitbuf >> 32 - N;
      this.FillBufNoEofCheck(N);
      return bitsNoEofCheck;
    }

    private uint GetBits17NoEofCheck(int N)
    {
      uint bits17NoEofCheck = this.dec_bitbuf >> 32 - N;
      this.FillBuf17NoEofCheck(N);
      return bits17NoEofCheck;
    }

    public enum BlockType
    {
      Invalid,
      Verbatim,
      Aligned,
      Uncompressed,
    }

    public enum DecoderState
    {
      Unknown,
      StartNewBlock,
      DecodingData,
    }
  }
}
