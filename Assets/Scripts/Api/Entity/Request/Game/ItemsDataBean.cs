namespace Hank.Api
{
    public class ItemsDataBean {
        /**
         * count : 12
         * name : nim
         * item_id : 1
         * type : 88
         */

        public int count{get;set;}
        public string name{get;set;}
        public int item_id{get;set;}
        public int type{get;set;}

        public ItemsDataBean(){}

        private ItemsDataBean(ItemsDataBeanBuilder builder) {
            setCount(builder.getCount());
            setName(builder.getName());
            setItem_id(builder.getItem_id());
            setType(builder.getType());
        }


        public int getCount() {
            return count;
        }

        public void setCount(int count) {
            this.count = count;
        }

        public string getName() {
            return name;
        }

        public void setName(string name) {
            this.name = name;
        }

        public int getItem_id() {
            return item_id;
        }

        public void setItem_id(int item_id) {
            this.item_id = item_id;
        }

        public int getType() {
            return type;
        }

        public void setType(int type) {
            this.type = type;
        }

        public static ItemsDataBeanBuilder getItemsDataBeanBuilder(){
            return new ItemsDataBeanBuilder();
        }

        public class ItemsDataBeanBuilder {
            private int count;
            private string name;
            private int item_id;
            private int type;

            public ItemsDataBeanBuilder() {
            }

            public int getCount() {
                return count;
            }

            public string getName() {
                return name;
            }

            public int getItem_id() {
                return item_id;
            }

            public int getType() {
                return type;
            }

            public ItemsDataBeanBuilder setCount(int val) {
                count = val;
                return this;
            }

            public ItemsDataBeanBuilder setName(string val) {
                name = val;
                return this;
            }

            public ItemsDataBeanBuilder setItem_id(int val) {
                item_id = val;
                return this;
            }

            public ItemsDataBeanBuilder setType(int val) {
                type = val;
                return this;
            }
            public ItemsDataBean build() {
                return new ItemsDataBean(this);
            }
        }
    }
}