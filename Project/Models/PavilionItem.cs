namespace Project.Models
{
    internal class PavilionItem
    {
        public long mall_id { get; set; } // id ТЦ
        public long mall_status_id { get; set; } // статус ТЦ
        public string mall_status_name { get; set; } // статус ТЦ
        public string mall_name { get; set; } // название ТЦ
        public int floor { get; set; } // номера этажа
        public string pavilion_number { get; set; } // номера павильона
        public decimal square { get; set; } // площадь
        public long pavilion_status_id { get; set; } // статус павильона
        public string pavilion_status_name { get; set; } // статус павильона
        public decimal cost_per_square_meter { get; set; } // cтоимость кв. м.
        public double value_added_factor { get; set; } // коэффициент добавочной стоимости павильона

        public static explicit operator Pavilion(PavilionItem pavilionItem)
        {
            return new Pavilion
            {
                mall_id = pavilionItem.mall_id,
                pavilion_number = pavilionItem.pavilion_number,
                floor = pavilionItem.floor,
                status_id = pavilionItem.pavilion_status_id,
                square = pavilionItem.square,
                cost_per_square_meter = pavilionItem.cost_per_square_meter,
                value_added_factor = pavilionItem.value_added_factor
            };
        }

        public override string ToString()
        {
            return $"{mall_name} {mall_status_name} {floor} {pavilion_number} {square} {pavilion_status_id} {pavilion_status_name} {cost_per_square_meter} {value_added_factor}";
        }
    }
}
